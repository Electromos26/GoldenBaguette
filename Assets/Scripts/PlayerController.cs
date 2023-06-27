using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : Unit
{
    [SerializeField]
    private CharacterController controller;

    private Camera playerCam; //this is the camera in our game
    private Gun[] gun = new Gun[1];


    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    GameManager gameManager;

    Vector3 velocity;
    bool isGrounded;


    protected override void Start()
    {
        
        playerCam = GetComponentInChildren<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        respawnPos = this.transform.position; //Change this to the checkpoint mechanic
        gun = GetComponentsInChildren<Gun>();
    }
    private Vector3 GetGunPosition()
    {
        return (gun[0].transform.position);//change from an array later line 12
        
    }
    void Update()
    {

        // pause here
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // dont jumpo when paused and dies 

        //if (gameManager.gameOver == false)//stop moving if the game ends
        //{

        //}

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= runSpeed;
            //Code for running animation
        }
        else
        {
            move *= speed;
            //Code for stop running animation
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("Crouching");
            //Code for crouching animation
        }

        controller.Move(move * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            //before we can show lasers going out into the infinite distance, we need to see if we are going to hit something
            LayerMask mask = ~LayerMask.GetMask("AISpot", "JeanRaider", "Ground");


            //we are having to do some ray casting
            Ray ray = new Ray(GetGunPosition(), playerCam.transform.forward); //aim our ray in the direction that we are looking
            RaycastHit hit; //our hit is going to be used as an output of a Raycast
            //so we need to use a layermask and a layermask is 
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                //if this is true, we hit something
                Attack(hit);
                Debug.Log("Got them");
            }
            else
            {
                //we now need to figure out a position we are firing
                Vector3 targetPos = GetGunPosition() +
                    playerCam.transform.forward * DISTANCE_SHOT_IF_NO_HIT;
                Debug.Log("pew");

            }
           
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



        //public void HandColorer()//get both player hands an change the colors 
        //{
        //    GameObject handL = GameObject.Find("hand");
        //    GameObject handR = GameObject.Find("hand1");

        //    MeshRenderer rendererL = handL.GetComponent<MeshRenderer>();
        //    MeshRenderer rendererR = handR.GetComponent<MeshRenderer>();

        //    rendererL.material.color = Color.red;
        //    rendererR.material.color = Color.blue;
        //}



    }
}

