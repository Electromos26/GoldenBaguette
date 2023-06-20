using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

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
   
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

