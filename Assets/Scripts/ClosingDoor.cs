using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;


    [SerializeField]
    private float doorSpeed = 1f;

    [SerializeField]
    private float openPositionY;

    private float closePositionY;

    [SerializeField]
    private bool doorCloser = false;

    [SerializeField]
    private bool ClosedDoor = false;

    [SerializeField]
    private bool CloseAfterOpening = false;
    BoxCollider boxCollider;
    void Start()
    {
        closePositionY = door.transform.position.y;
    }

    public void OpenDoor()
    {
        if (ClosedDoor)
        {
            StartCoroutine(OpenDoors());

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && doorCloser)//make sure its the player
        {
            ClosedDoor = true;
            StartCoroutine(CloseDoor());
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
        if (!CloseAfterOpening)
        {
            // Disable the collider component of the door to allow walking through
            Collider doorCollider = GetComponent<Collider>();
            if (doorCollider != null)
                doorCollider.enabled = false;
        }
        else
        {
            doorCloser = true;
        }


        // Door transition complete
    }
    private IEnumerator CloseDoor()
    {

        closePositionY = -openPositionY + 1f;
        Vector3 initialPosition = door.transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, closePositionY, initialPosition.z);

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
        ClosedDoor = true;
        // door animation complete
    }


}


