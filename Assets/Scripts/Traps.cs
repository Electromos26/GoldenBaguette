using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField]
    private GameObject trap;
    [SerializeField]
    private GameObject endOfTrap; // Insert trap Out location
    [SerializeField]
    private float trapSpeed = 20f;
    private Vector3 trapPosition;

    private bool playerInCollider = false;
    
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        trapPosition = endOfTrap.transform.position;
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInCollider = true;

        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (playerInCollider)
        {
            // Trigger the traps
            StartCoroutine(TriggerTrapAnimation());
            // Perform any necessary actions for player death
        }


    }

    private IEnumerator TriggerTrapAnimation()
    {
        if (playerInCollider)
        {
            trap.SetActive(true);

            Vector3 initialPosition = trap.transform.position;
            float distance = Vector3.Distance(initialPosition, trapPosition);
            float duration = distance / trapSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / duration;

                trap.transform.position = Vector3.Lerp(initialPosition, trapPosition, normalizedTime);

                yield return null;
            }
        }
       
        player.PublicDie();
        Debug.Log("You died.");

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInCollider = false;
        }
    }
}