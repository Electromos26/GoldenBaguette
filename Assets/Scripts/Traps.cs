using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    public int trapDamage;

    private float timer;

    [SerializeField]
    private float damageTimer;

    private bool playerInCollider = false;
    private PlayerController player;

    public bool TrapActive;

    [SerializeField]
    private bool floorTrap;

    // Start is called before the first frame update
    void Start()
    {
        trapPosition = endOfTrap.transform.position;
        player = GameObject.FindObjectOfType<PlayerController>();
        timer = damageTimer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && floorTrap)
        {
            playerInCollider = true;
            TrapActive = true;

        }
    }
    private void Update()
    {

        if (playerInCollider || TrapActive)
        {
            // Trigger the traps
            StartCoroutine(TriggerTrapAnimation());
            // Perform any necessary actions for player death

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (TrapActive)
        {
            timer += Time.deltaTime;
            if (timer > damageTimer)
            {
                player.OnTrapHit(trapDamage);
                //player.PublicDie();
                timer = 0;
            }
        }


    }

    private IEnumerator TriggerTrapAnimation()
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


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInCollider = false;
        }
    }
}