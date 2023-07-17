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
    private Vector3 startPosition;

    public int trapDamage;

    private float timer;

    [SerializeField]
    private float damageTimer;

    private PlayerController player;

    public bool TrapActive;

    [SerializeField]
    private bool floorTrap;

    [SerializeField]
    private bool loopTrap;

    [SerializeField]
    private float loopInterval = 2f;

    private float loopTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = trap.transform.position;
        trapPosition = endOfTrap.transform.position;
        player = GameObject.FindObjectOfType<PlayerController>();
        timer = damageTimer;
        loopTimer = loopInterval;

        if (TrapActive)
        {
            // Set trap position to end of trap if the trap is active
            trap.transform.position = trapPosition;
        }

    }

    private void OnTriggerEnter(Collider other) //Sets trap when the player is in collider and does damage to it
    {
        if (other.CompareTag("Player") && floorTrap)
        {
            StartCoroutine(TriggerTrapAnimation());
            TrapActive = true;
        }
    }
    private void Update()
    {

        if (loopTrap)
        {
            loopTimer += Time.deltaTime;
            if (loopTimer > loopInterval && !TrapActive)
            {
                StartCoroutine(TriggerTrapAnimation());
                loopTimer = 0;
            }

            if (loopTimer > loopInterval && TrapActive)
            {
                StartCoroutine(RetractTrapAnimation());
                loopTimer = 0;
            }

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
        TrapActive = true;

        float distance = Vector3.Distance(startPosition, trapPosition);
        float duration = distance / trapSpeed;
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            trap.transform.position = Vector3.Lerp(startPosition, trapPosition, normalizedTime);

            yield return null;
        }

    }

    private IEnumerator RetractTrapAnimation()
    {


        float distance = Vector3.Distance(startPosition, trapPosition);
        float duration = distance / trapSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            trap.transform.position = Vector3.Lerp(trapPosition, startPosition, normalizedTime);

            yield return null;
        }

        TrapActive = false;
        trap.SetActive(false);


    }


}