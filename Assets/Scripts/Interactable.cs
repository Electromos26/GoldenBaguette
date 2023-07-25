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

    private PlayerController player;


    private Vector3 pressPosition;

    private Vector3 initialPosition;

    private bool buttonPressed = false;

    private float timer;

    [SerializeField]
    private float pressedInterval = 20f;

    [SerializeField]
    private UnityEvent events;

    private void Start()
    {
        initialPosition = button.transform.position;
        pressPosition = endOfPress.transform.position;
        player = GameObject.FindObjectOfType<PlayerController>();

    }

    private void Update()
    {
        if (buttonPressed)
        {
            timer += Time.deltaTime;

            if (timer > pressedInterval || !player.isAlive)
            {
                StartCoroutine(RetractAnimation());
                timer = 0;
                buttonPressed = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))//make sure its the player
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                events.Invoke();
                // Trigger the traps
                StartCoroutine(PressedAnimation());
                // StartCoroutine(TriggerTrapAnimation());
                buttonPressed = true;
            }
        }

        
    }
    private IEnumerator PressedAnimation()
    {
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
        // Button animation complete
    }

    private IEnumerator RetractAnimation()
    {
        float distance = Vector3.Distance(initialPosition, pressPosition);
        float duration = distance / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            button.transform.position = Vector3.Lerp(pressPosition, initialPosition, normalizedTime);

            yield return null;
        }
        // Retract animation complete
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


