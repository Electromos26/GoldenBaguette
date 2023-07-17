using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject button;//for animations
    [SerializeField]
    private GameObject endOfPress;//insert button location
    private Vector3 pressPosition;

    [SerializeField]
    private UnityEvent events;

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
                events.Invoke();
                // Trigger the traps
                StartCoroutine(PressedAnimation());
                // StartCoroutine(TriggerTrapAnimation());
            }
        }

        
    }
    private IEnumerator PressedAnimation()
    {
        Vector3 initialPosition = button.transform.position;
        float distance = Vector3.Distance(initialPosition, pressPosition);
        float duration = distance /  2f;
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
    }



   /* private IEnumerator TriggerTrapAnimation()
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

            

        }
    }*/
   
}


