using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Unity.PlasticSCM.Editor;
using Unity.VisualScripting;

public class PlayerController : Unit
{
    [SerializeField]
    GameManager gameManager;

    #region CharacterControllerSettings
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float controllerCenter = 0.95f;

    [SerializeField]
    private float crouchCenterController;

    [SerializeField]
    private float crouchHeightController = 1.4f;

    [SerializeField]
    private float defaultHeightController = 1.9f;

    private Vector3 defaultCenterVector;
    private Vector3 crouchCenterVector;
    #endregion

    #region PlayerMovement
    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;

    Vector3 velocity;
    bool isGrounded;

    [SerializeField]
    private Transform groundCheck;

    private float groundDistance = 0.4f;

    [SerializeField]
    private LayerMask groundMask;

    private float turnSmoothVelocity;

    [SerializeField]
    private float turnSmoothTime = 0.2f;
    #endregion

    #region ZoomAim
    private float defaultView;

    [SerializeField]
    private float zoomIn = 3f;

    [SerializeField]
    private float zoomSmooth;

    [SerializeField]
    Laser laserPrefab;

    [SerializeField]
    private GameObject crossHair;

    #endregion

    private const float ANIMATOR_SMOOTHING = 0.4f;

    private Vector2 animatorInput;

    [SerializeField]
    private GameObject gun;

    private AIController AIScript;

    private float AILookDistanceDefault;

 //   private bool pickedUp = false;

    private Camera playerCam; //this is the camera in our game

    [SerializeField]
    private AudioClip _walkClip;

    private HealthBar healthBar;

    [SerializeField]
    private GameObject baguetteIcon;

    public DeathMenu PlayerIs;
    protected override void Start()
    {
        base.Start();
        playerCam = GetComponentInChildren<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        respawnPos = this.transform.position; //Change this to the checkpoint mechanic

        AIScript = GameObject.FindObjectOfType<AIController>();

        defaultView = playerCam.fieldOfView;
        crouchCenterVector = new Vector3(0, crouchCenterController);
        defaultCenterVector = new Vector3(0, controllerCenter);
        AILookDistanceDefault = AIScript.lookDistance;

        healthBar.SetMaxHealth(fullHealth);

    }
    private void ShowLasers(Vector3 targetPosition) //the target position is what we are aiming for
    {

        Laser laser = Instantiate(laserPrefab) as Laser; //the "as Laser" casts the game object to a laser; this is a technique we can use if we know we are creating a game object of a specific type (in this case, we know the laserPrefab is going to be a Laser)
        laser.Init(Color.red, gun.transform.position, targetPosition);

    }

    private Vector3 GetGunPosition()
    {
        return (gun.transform.position);//change from an array later line 12

    }
    void Update()
    {
        healthBar.SetHealth(health);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isAlive)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector2 input = new Vector2(x, z);
            Vector2 inputDir = input.normalized;

            // dont move when paused

            //if (gameManager.gameOver == false)//stop moving if the game ends
            //{

            //}

            //Movement
            Vector3 move = transform.right * x + transform.forward * z;

            //Adjusting player movement considering camera position
            if (inputDir != Vector2.zero)
            {

                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) + playerCam.transform.eulerAngles.y;

                transform.eulerAngles = new Vector3 (0 , Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime), 0);
            }

            controller.center = defaultCenterVector; //Centering the controller on the playyer to adjust collision

            animatorInput = Vector2.Lerp(animatorInput, inputDir, ANIMATOR_SMOOTHING);
            animator.SetFloat("HorizontalSpeed", animatorInput.x);
            animator.SetFloat("VerticalSpeed", animatorInput.y);

            //Every movement and animation the player does
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetButton("Fire2")) //Logic for the player to run
            {
                move *= runSpeed;
                //Code for running animation
                animator.SetBool("Crouching", false);
                animator.SetBool("AimCrouching", false);
                animator.SetBool("Running", true);
                animator.SetBool("Aiming", false);

                AIScript.lookDistance = AILookDistanceDefault;

                controller.center = defaultCenterVector;
                controller.height = defaultHeightController;
                crossHair.SetActive(false);
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies

            }
            else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetButton("Fire2")) //Logic for the player to crouch and walk around
            {
                move *= crouchSpeed;

                animator.SetBool("Crouching", true);
                animator.SetBool("AimCrouching", false);
                animator.SetBool("Running", false);
                animator.SetBool("Aiming", false);

                AIScript.lookDistance = AILookDistanceDefault / 2;

                controller.center = crouchCenterVector;
                controller.height = crouchHeightController;
                crossHair.SetActive(false);
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetButton("Fire2")) //Logic for crouching and shooting, player cant move around
            {
                move *= 0;
                animator.SetBool("Crouching", false);
                animator.SetBool("AimCrouching", true);
                animator.SetBool("Running", false);
                animator.SetBool("Aiming", false);

                AIScript.lookDistance = AILookDistanceDefault / 2;

                controller.center = crouchCenterVector;
                controller.height = crouchHeightController;
                AimingAndShooting();
            }
            else if (Input.GetButton("Fire2"))//Logic for the aim on while walking
            {
                move *= speed;
                animator.SetBool("Crouching", false);
                animator.SetBool("AimCrouching", false);
                animator.SetBool("Running", false);
                animator.SetBool("Aiming", true);
                AIScript.lookDistance = AILookDistanceDefault;

                AimingAndShooting();
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) //Logic for the player to walk without the aim on
            {
                move *= speed;
                animator.SetBool("Crouching", false);
                animator.SetBool("AimCrouching", false);
                animator.SetBool("Running", false);
                animator.SetBool("Aiming", false);

                AIScript.lookDistance = AILookDistanceDefault;

                controller.center = defaultCenterVector;
                controller.height = defaultHeightController;
                crossHair.SetActive(false);
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies
            }

            controller.Move(move * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded) //Jumping
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jumping");
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (isGrounded && (move.z > 0 || move.x > 0) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift)) //Play walking sound when player is moving
            {

                if (_audioSource != null && !_audioSource.isPlaying)
                {
                    _audioSource.clip = _walkClip;
                    _audioSource.loop = true;
                    _audioSource.Play();
                }

            }
            else if ((isGrounded && (move.z > 0 || move.x > 0) && !Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)))
            {
                _audioSource.Stop();
                if (_audioSource != null && !_audioSource.isPlaying)
                {
                    _audioSource.clip = _runClip;
                    _audioSource.loop = true;
                    _audioSource.Play();
                }

            }
            else
            {
                _audioSource.loop = false;
                //_audioSource.Stop();
            }


        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies
        }

    }

    private void OnTriggerEnter(Collider other)//Checking triggers on player
    {
        if (other.tag == "Checkpoint") //Checkpoint collision
        {
            respawnPos = transform.position; //Setting the respawnPos to the position of the checkpoint
            Destroy(other.gameObject);
        }

        if (other.gameObject.GetComponent<Collectable>()) // Check if the object collided is a collectible
        {
            Debug.Log("Triggered");
            Collectable collectableObject = other.gameObject.GetComponent<Collectable>();
            //gameManager.IncreaseScore(collectableObject.GetNumPoints());
          //  pickedUp = true;
            if (other.gameObject.name == "Golden_Baguette")
            {
                baguetteIcon.SetActive(true);
            }

            if (other.gameObject.name == "Tape")
            {
                collectableObject.PlayTrack();
            }

            other.gameObject.SetActive(false);


        }

    }

    private void AimingAndShooting() //Function to maike the player aim and be able to shoot
    {
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView / zoomIn, Time.deltaTime * zoomSmooth); //Reduces field of view for zoom
        transform.eulerAngles = new Vector3 (0, playerCam.transform.eulerAngles.y,0);
        crossHair.SetActive(true); //Activate crosshair

        if (Input.GetButtonDown("Fire1"))
        {
            if (_audioSource != null)
            {
                _audioSource.clip = _attackClip;
                _audioSource.Play();
            }

            //before we can show lasers going out into the infinite distance, we need to see if we are going to hit something
            LayerMask mask = ~LayerMask.GetMask("AISpot", "JeanRaider", "Ground", "Interactables");


            //we are having to do some ray casting
            Ray ray = new Ray(GetGunPosition(), playerCam.transform.forward); //aim our ray in the direction that we are looking
            RaycastHit hit; //our hit is going to be used as an output of a Raycast
                            //so we need to use a layermask and a layermask is 
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                //if this is true, we hit something
                Attack(hit);
                Debug.Log("Got them");
                ShowLasers(hit.point);
            }
            else
            {
                //we now need to figure out a position we are firing
                Vector3 targetPos = GetGunPosition() + playerCam.transform.forward * DISTANCE_SHOT_IF_NO_HIT;
                ShowLasers(targetPos);
            }

        }

    }

    protected override void Die()
    {
        animator.SetBool("Crouching", false);
        animator.SetBool("AimCrouching", false);
        animator.SetBool("Running", false);
        animator.SetBool("Aiming", false);
        Debug.Log("wow i died");
        base.Die();
        if (PlayerIs != null)
        {
            // Start the coroutine to wait before setting isDead to true
            StartCoroutine(DelayBeforeDeath());
        }

    }
    private IEnumerator DelayBeforeDeath()
    {
        yield return new WaitForSeconds(4f); // Wait for 5 seconds
        PlayerIs.isDead = true;
    }

    //private void OnTriggerEnter(Collider other) // collectables
    //{
    //    if (other.gameObject.GetComponent<GoldCoin>())//special collectible with goldCoin script  
    //    {
    //        //GoldCoin collectableObject = other.gameObject.GetComponent<GoldCoin>();
    //        gameManager.IncreaseScore(collectableObject.GetNumPoints());
    //        Destroy(other.gameObject);
    //        HandColorer();
    //    }
    //    else if (other.gameObject.GetComponent<Collectible>()) // every other collectable
    //    {
    //        Collectible collectableObject = other.gameObject.GetComponent<Collectible>();
    //        gameManager.IncreaseScore(collectableObject.GetNumPoints());
    //        Destroy(other.gameObject);
    //    }

    //}

}

