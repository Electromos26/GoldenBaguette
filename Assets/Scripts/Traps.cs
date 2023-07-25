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

    private float retractTimer;

    [SerializeField]
    private float damageTimer;

    private PlayerController player;

    private Boss boss;

    public bool trapActive;

    [SerializeField]
    private bool floorTrap;

    [SerializeField]
    private bool loopTrap;

    private bool alwaysOn;

    [SerializeField]
    private float loopInterval = 2f;

    private float loopTimer;

    [SerializeField]
    private float activeInterval = 20f;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = trap.transform.position;
        trapPosition = endOfTrap.transform.position;
        player = GameObject.FindObjectOfType<PlayerController>();
        boss = GameObject.FindObjectOfType<Boss>();
        timer = damageTimer;
        loopTimer = loopInterval;

        if (trapActive)
        {
            // Set trap position to end of trap if the trap is active
            trap.transform.position = trapPosition;
            trap.SetActive(true);
            alwaysOn = true;
        }

    }

    private void OnTriggerEnter(Collider other) //Sets trap when the player is in collider and does damage to it
    {
        if (other.CompareTag("Player") && floorTrap)
        {
            StartCoroutine(TriggerTrapAnimation());
            trapActive = true;
        }

        if (other.CompareTag("Boss") && trapActive)
        {
            boss.GotStunned();
            Debug.Log("Stunned");
        }
    }
    private void Update()
    {
        if (trapActive && !alwaysOn)
        {
           
            retractTimer += Time.deltaTime;

                if (retractTimer > activeInterval || !player.isAlive)
                {
                    StartCoroutine(RetractTrapAnimation());
                    retractTimer = 0;
                }
            
        }


        if (loopTrap)
        {
            loopTimer += Time.deltaTime;
            if (loopTimer > loopInterval && !trapActive)
            {
                StartCoroutine(TriggerTrapAnimation());
                loopTimer = 0;
            }

            if (loopTimer > loopInterval && trapActive)
            {
                StartCoroutine(RetractTrapAnimation());
                loopTimer = 0;
            }

        }
        
    }

    public void ButtonPressed()
    {
        StartCoroutine(TriggerTrapAnimation());
    }

    private void OnTriggerStay(Collider other)
    {
        if (trapActive && other.CompareTag("Player") && player.isAlive)
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

    public IEnumerator TriggerTrapAnimation()
    {

        trap.SetActive(true);
        trapActive = true;

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

        if (!player.isAlive)
        {
            yield return new WaitForSeconds(2f); // Wait for 5 seconds
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            trap.transform.position = Vector3.Lerp(trapPosition, startPosition, normalizedTime);

            yield return null;
        }
       
        trapActive = false;
        trap.SetActive(false);
        timer = damageTimer;
    }


}