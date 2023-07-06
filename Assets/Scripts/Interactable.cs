using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject button;//for animations
    [SerializeField]
    private GameObject endOfPress;//insert button location
    private Vector3 pressPosition;

    [SerializeField]
    private GameObject[] doors;
    [SerializeField]
    private float openPositionY;
    [SerializeField]
    private float doorSpeed = 1f;

    [SerializeField]
    private GameObject[] traps;
    [SerializeField]
    private float trapOutY;
    [SerializeField]
    private float trapSpeed = 20f;
    private Vector3 trapPosition;


    private void Start()
    {

    }
    private void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))//make sure its the player
        {
            if (Input.GetKeyDown(KeyCode.E) && endOfPress != null)
            {
                pressPosition = endOfPress.transform.position;
                // Trigger the traps

                
                StartCoroutine(PressedAnimation());
                StartCoroutine(OpenDoors());
                StartCoroutine(TriggerTrapAnimation());
            }
        }
    }
    private IEnumerator PressedAnimation()
    {
        Vector3 initialPosition = button.transform.position;
        float distance = Vector3.Distance(initialPosition, pressPosition);
        float duration = distance / (doorSpeed / 2f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            button.transform.position = Vector3.Lerp(initialPosition, pressPosition, normalizedTime);

            yield return null;
        }
        Destroy(endOfPress);
        // Button animation complete
        Debug.Log("Button animation complete");
    }



    private IEnumerator OpenDoors()
    {
        foreach (var door in doors)
        {
            Vector3 initialPosition = door.transform.position;
            Vector3 targetPosition = new Vector3(initialPosition.x, openPositionY, initialPosition.z);

            float distance = Vector3.Distance(initialPosition, targetPosition);
            float duration = distance / doorSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

                door.transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

                yield return null;
            }

            // Disable the collider component of the door to allow walking through
            Collider doorCollider = door.GetComponent<Collider>();
            if (doorCollider != null)
                doorCollider.enabled = false;

            // Door transition complete
            Debug.Log("The door is fully open");
        }
    }
    private IEnumerator TriggerTrapAnimation()
    {
       
        foreach (var trap in traps)
        {
            trap.SetActive(true);
            Vector3 initialPosition = trap.transform.position;
            Vector3 targetPosition = new Vector3(initialPosition.x, trapOutY, initialPosition.z);

            float distance = Vector3.Distance(initialPosition, targetPosition);
            float duration = distance / trapSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

                trap.transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

                yield return null;
            }

            Debug.Log("You died.");

        }
    }
   
}


