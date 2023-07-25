using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;
    [SerializeField]
    private float Invert = -1f;

    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
    }
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
     
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * Invert;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -40f, 40f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        
    }
}