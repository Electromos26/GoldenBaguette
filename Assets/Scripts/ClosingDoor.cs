using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;


    [SerializeField]
    private float doorSpeed = 1f;

    private float openPositionY;

    private float closePositionY;

    [SerializeField]
    private bool doorCloser = false;

    [SerializeField]
    private bool ClosedDoor = false;

    [SerializeField]
    private bool CloseAfterOpening = false;
    BoxCollider boxCollider;

    private AudioSource _audioSource;

    void Start()
    {
        openPositionY = 3f;
        closePositionY = -0.2f;

        _audioSource = GetComponent<AudioSource>();


        if (ClosedDoor)
        {
            door.transform.localPosition = new Vector3(door.transform.localPosition.x, closePositionY, door.transform.localPosition.z);
        }
        else
        {
            door.transform.localPosition = new Vector3(door.transform.localPosition.x, openPositionY, door.transform.localPosition.z);
        }
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
        Vector3 initialPosition = door.transform.localPosition;
        Vector3 targetPosition = new Vector3(initialPosition.x, openPositionY, initialPosition.z);

        float distance = Vector3.Distance(initialPosition, targetPosition);
        float duration = distance / doorSpeed;
        float elapsedTime = 0f;

        if (_audioSource != null)
        {
            _audioSource.Play();
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

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            door.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

            yield return null;
        }


        // Door transition complete
    }
    private IEnumerator CloseDoor()
    {

        Vector3 initialPosition = door.transform.localPosition;
        Vector3 targetPosition = new Vector3(initialPosition.x, closePositionY, initialPosition.z);

        float distance = Vector3.Distance(initialPosition, targetPosition);
        float duration = distance / doorSpeed;
        float elapsedTime = 0f;

        if (_audioSource != null)
        {
            _audioSource.Play();
        }


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            door.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);
            
            yield return null;
        }
        ClosedDoor = true;
        // door animation complete
    }


}


