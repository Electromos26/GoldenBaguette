using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{

    Vector3 FollowPOS;
    public float clampAngle = 80.0f;
    public float mouseSensitivity = 150.0f;
    public Transform PlayerObj;
    public float camDistanceToPlayer;
    public float rotationSmoothTime = 1.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

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
        rotX += finalInputZ * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(rotX, rotY), ref rotationSmoothVelocity, rotationSmoothTime);

        Vector3 targetRotation = new Vector3(rotX, rotY);
        transform.eulerAngles = targetRotation;

        transform.position = PlayerObj.position - transform.forward * camDistanceToPlayer;


    }

}
