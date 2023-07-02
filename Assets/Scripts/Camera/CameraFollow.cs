using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private float clampAngle = 80.0f;
    [SerializeField]
    private float mouseSensitivity = 150.0f;
    [SerializeField]
    private Transform PlayerObj;


    private float camDistanceToPlayer;
    [SerializeField]
    private float rotationSmoothTime = 1.2f;


  /*  Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;*/

    private float mouseX;
     float mouseY;
     float finalInputX;
     float finalInputZ;
     float smoothX;
     float smoothY;
     float rotY = 0.0f;
     float rotX = 0.0f;

    [SerializeField]
    private float Invert = -1f;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        // We setup the rotation of the sticks here
        //float inputX = Input.GetAxis("RightStickHorizontal");
        //float inputZ = Input.GetAxis("RightStickVertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * mouseSensitivity * Time.deltaTime;
        rotX += finalInputZ * mouseSensitivity * Time.deltaTime * Invert;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(rotX, rotY), ref rotationSmoothVelocity, rotationSmoothTime);

        Vector3 targetRotation = new Vector3(rotX, rotY);
        transform.eulerAngles = targetRotation;

        transform.position = PlayerObj.position - transform.forward * camDistanceToPlayer;


    }

}