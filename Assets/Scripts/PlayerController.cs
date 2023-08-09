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
using UnityEngine.UI;
using TMPro;
/// <summary>
/// There is a lot of really good work here and I think you have done a good job; however, you need to break code into smaller chunks and smaller functions. You have some functions here that are hundreds of lines and they are extremely difficult to follow. For both your sake, and mine, break these into smaller more manageable pieces. Also, you should not be having the GameManager as a serializable field. A game manager should be a singleton that is accessible in all classes, and so the way you have done it you are specify a GameManager in ways that you do not need to. 
/// </summary>
public class PlayerController : Unit
{
    GameManager gameManager;

    RoomManager roomManager;

    AudioManager audioManager;

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

    private Vector3 move;

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
    [SerializeField]
    private AudioClip _crouchWalkClip;

    private HealthBar healthBar;

    [SerializeField]
    private GameObject baguetteIcon;

    [SerializeField]
    private GameObject reloadIcon;

    [SerializeField]
    private TMP_Text ammoText;

    private Gun gunScript;

    public DeathMenu PlayerIs;

    private int shotCount = 0;

    [SerializeField]
    private int shotLimit = 5;

    [SerializeField]
    private float reloadTime;

    private float timer;

    private bool isReloading = false;

    private enum State
    {
        Running,
        WalkCrouching,
        AimCrouching,
        AimWalking,
        Walking
    }

    private State currentState; //this keeps track of the current state




    protected override void Start()
    {
        base.Start();
        playerCam = GetComponentInChildren<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        gunScript = gun.GetComponent<Gun>();

        respawnPos = this.transform.position;

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

    private Vector3 GetCamPosition()
    {
        return (playerCam.transform.position);//change from an array later line 12
    }

    private void SetState(State newState)
    {
        //what we want to do here is look at the newstater, compare it to the enumvalues, and then figure out what to do based on that.
        //set state will only be called when a state changes
        currentState = newState;
        StopAllCoroutines();//stop the previous coroutines so they aren't operating at the same time
        switch (currentState)
        {
            case State.Running:
                StartCoroutine(OnRunning());
                //do some work
                break;
            case State.WalkCrouching:
                StartCoroutine(OnWalkCrouching());
                //do some work
                break;
            case State.AimCrouching:
                StartCoroutine(OnAimCrouching());
                //do some work
                break;
            case State.AimWalking:
                StartCoroutine(OnAimWalking());
                break;
            case State.Walking:
                StartCoroutine(OnWalking());
                break;
            default:
                break;
        }
        ///
    }


    void Update()
    {
        healthBar.SetHealth(health);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        ammoText.text = (shotLimit - shotCount).ToString();


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
            move = transform.right * x + transform.forward * z;

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
                SetState(State.Running);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetButton("Fire2")) //Logic for the player to crouch and walk around
            {
                SetState(State.WalkCrouching);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetButton("Fire2")) //Logic for crouching and shooting, player cant move around
            {
                SetState(State.AimCrouching);
            }
            else if (Input.GetButton("Fire2"))//Logic for the aim on while walking
            {
                SetState(State.AimWalking);
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) //Logic for the player to walk without the aim on
            {
                SetState(State.Walking);
            }

            controller.Move(move * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded) //Jumping
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jumping");
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (move.z == 0 && move.x == 0)
            {
                _audioSource.loop = false;
                _audioSource.Stop();
            }


            if (Input.GetKeyDown(KeyCode.R) || (shotCount >= shotLimit && !isReloading))
            {
                gunScript.ReloadSound();
                isReloading = true;
            }

            if (isReloading)
            {

                if (timer < reloadTime)
                {
                    timer += Time.deltaTime;
                    reloadIcon.SetActive(true);
                }
                else
                {
                    timer = 0;
                    shotCount = 0;
                    reloadIcon.SetActive(false);
                    isReloading = false;
                }

            }


        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies
        }
    }


    #region PlayerStates

    private IEnumerator OnRunning()
    {
        move *= runSpeed;
        //Code for running animation
        DisableAllAnimations();
        animator.SetBool("Running", true);

        AIScript.lookDistance = AILookDistanceDefault;

        int runClipArr = UnityEngine.Random.Range(0, _runClip.Length);

        if (isGrounded && (move.z > 0 || move.x > 0))
        {
            if (_audioSource.clip != _runClip[runClipArr])
            {
                _audioSource.Stop();
            }
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _audioSource.clip = _runClip[runClipArr];
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        controller.center = defaultCenterVector;
        controller.height = defaultHeightController;
        crossHair.SetActive(false);
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies

        yield return null;
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);

        
    }

    private IEnumerator OnWalking()
    {
        move *= speed;
        DisableAllAnimations();
        AIScript.lookDistance = AILookDistanceDefault;

        if (isGrounded && (move.z > 0 || move.x > 0)) //Play walking sound when player is moving
        {
            if (_audioSource.clip != _walkClip)
            {
                _audioSource.Stop();
            }
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _audioSource.clip = _walkClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        controller.center = defaultCenterVector;
        controller.height = defaultHeightController;
        crossHair.SetActive(false);
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies
        
        yield return null;
    }

    private IEnumerator OnWalkCrouching()
    {
        move *= crouchSpeed;

        DisableAllAnimations();
        animator.SetBool("Crouching", true);

        AIScript.lookDistance = AILookDistanceDefault / 2;

        if (isGrounded && (move.z > 0 || move.x > 0))
        {
            if (_audioSource.clip != _crouchWalkClip)
            {
                _audioSource.Stop();
            }
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _audioSource.clip = _crouchWalkClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        controller.center = crouchCenterVector;
        controller.height = crouchHeightController;
        crossHair.SetActive(false);
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView, Time.deltaTime * zoomSmooth); //Return camera to deafult view if the player dies

        yield return null;
    }

    private IEnumerator OnAimCrouching()
    {
        move *= 0;

        DisableAllAnimations();
        animator.SetBool("AimCrouching", true);

        AIScript.lookDistance = AILookDistanceDefault / 2;

        controller.center = crouchCenterVector;
        controller.height = crouchHeightController;
        AimingAndShooting();

        yield return null;
    }

    private IEnumerator OnAimWalking()
    {
        move *= speed;

        DisableAllAnimations();
        animator.SetBool("Aiming", true);
        AIScript.lookDistance = AILookDistanceDefault;

        if (isGrounded && (move.z > 0 || move.x > 0)) //Play walking sound when player is moving
        {
            if (_audioSource.clip != _walkClip)
            {
                _audioSource.Stop();
            }
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _audioSource.clip = _walkClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        AimingAndShooting();

        yield return null;
    }


    #endregion


    private void OnTriggerEnter(Collider other)//Checking triggers on player
    {
        if (other.tag == "Checkpoint") //Checkpoint collision
        {
            respawnPos = transform.position; //Setting the respawnPos to the position of the checkpoint

            roomManager.EnterRoom(other.GetComponentInParent<Room>());
            Destroy(other.gameObject);
        }

        if (other.gameObject.GetComponent<Collectable>()) // Check if the object collided is a collectible
        {
            Collectable collectableObject = other.gameObject.GetComponent<Collectable>();
            //gameManager.IncreaseScore(collectableObject.GetNumPoints());
            //pickedUp = true;
            if (other.gameObject.name == "Golden_Baguette")
            {
                baguetteIcon.SetActive(true);
            }

            if (other.gameObject.tag == "Tape")
            {
                collectableObject.PlayTrack();
                audioManager.PlayFocusedAudio(collectableObject.GetComponent<AudioSource>());
            }

            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

    }

    private void AimingAndShooting() //Function to make the player aim and be able to shoot
    {
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defaultView / zoomIn, Time.deltaTime * zoomSmooth); //Reduces field of view for zoom
        transform.eulerAngles = new Vector3 (0, playerCam.transform.eulerAngles.y,0);
        crossHair.SetActive(true); //Activate crosshair

        if (Input.GetButtonDown("Fire1") && shotCount < shotLimit && !isReloading)
        {

            shotCount++;

            gunScript.PlayShootSound();
            //before we can show lasers going out into the infinite distance, we need to see if we are going to hit something
            LayerMask mask = ~LayerMask.GetMask("AISpot", "JeanRaider", "Ground", "Interactables");

            //we are having to do some ray casting
            Ray ray = new Ray(GetCamPosition(), playerCam.transform.forward); //aim our ray in the direction that we are looking
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
                Vector3 targetPos = GetCamPosition() + playerCam.transform.forward * DISTANCE_SHOT_IF_NO_HIT;
                ShowLasers(targetPos);
            }

        }

    }

    private void DisableAllAnimations()
    {
        animator.SetBool("Crouching", false);
        animator.SetBool("AimCrouching", false);
        animator.SetBool("Running", false);
        animator.SetBool("Aiming", false);
    }

    protected override void Die()
    {
        DisableAllAnimations();
        base.Die();
        if (PlayerIs != null)
        {
            // Start the coroutine to wait before setting isDead to true
            //StartCoroutine(DelayBeforeDeath());
            PlayerIs.Die();
        }
        shotCount = 0;
    }
/*    private IEnumerator DelayBeforeDeath()
    {
        yield return new WaitForSeconds(0f); // Wait for 4 seconds
        PlayerIs.isDead = true;
    }
*/
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

