using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
  //  private bool isPlayerInside; //use if code breaks to be 100% sure its in box

    void Start()
    {

    }
    void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
       //     isPlayerInside = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Player is inside the trigger box and pressed the Space button
                Debug.Log("button pressed");

                doorOpen();
            }
        }
    }
    protected void doorOpen()
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //   isPlayerInside = false;
            Debug.Log("left the area cant press button");

        }
    }
}
