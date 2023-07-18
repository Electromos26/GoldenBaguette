using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject endOfDoorClose;


    [SerializeField]
    private float doorSpeed = 1f;

    [SerializeField]
    private float openPositionY;
    private Vector3 closePositionY;

    [SerializeField]
    private bool playerDoorCloser = false;

    [SerializeField]
    private bool closeDoor = false;

    BoxCollider boxCollider;
    void Start()
    {
    
        closePositionY = endOfDoorClose.transform.position;
    }

    public void OpenDoor()
    {
        if (closeDoor)
        {
            StartCoroutine(OpenDoors());

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerDoorCloser)//make sure its the player
        {
            closeDoor = true;
            StartCoroutine(CloseAnimation());
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

        }
    }
    private IEnumerator OpenDoors()
    {
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, openPositionY, initialPosition.z);

        float distance = Vector3.Distance(initialPosition, targetPosition);
        float duration = distance / doorSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

            yield return null;
        }

        // Disable the collider component of the door to allow walking through
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
            doorCollider.enabled = false;

        // Door transition complete
    }
    private IEnumerator CloseAnimation()
    {
        

            Vector3 initialPosition = door.transform.position;
            float distance = Vector3.Distance(initialPosition, closePositionY);
            float duration = distance / (doorSpeed);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / duration;

                door.transform.position = Vector3.Lerp(initialPosition, closePositionY, normalizedTime);

                yield return null;
            }
            // door animation complete
            Debug.Log("door animation complete");

       
    }


}


