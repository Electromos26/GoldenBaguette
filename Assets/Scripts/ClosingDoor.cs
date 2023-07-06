using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject endOfDoorClose;

    private Vector3 closePositionY;

    [SerializeField]
    private float doorSpeed = 1f;

    private bool closeDoor;

    BoxCollider boxCollider;
    void Start()
    {
    
        closePositionY = endOfDoorClose.transform.position;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//make sure its the player
        {
            Debug.Log("his in");
            closeDoor = true;
            StartCoroutine(CloseAnimation());
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

        }
    }
    private IEnumerator CloseAnimation()
    {

        while (closeDoor) 
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

            closeDoor = false;
        }
       
    }


}


