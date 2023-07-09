using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AISpot : MonoBehaviour
{
    //when a unit enters and stays within our area, after a certain interval we want to allow them to start collecting points (essentially, they captured the flag)
    [SerializeField]
    private float timer; //this will keep track of the time within the outpost

    [SerializeField]
    internal int team;//this keeps track of the current team (-1 being no one). internal is public to the project


    [SerializeField] float waitingTimeMax = 5.0f;//how long for them to increase a value
    [SerializeField] float waitingTimeMin = 2.0f;//how long for them to increase a value

    internal float currentValue = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Timer to check if the AI can move to the next outpost

    }

    private void OnTriggerStay(Collider other)
    {
        AIController AI = other.GetComponent<AIController>();

        if (AI != null)
        {
            Debug.Log("AI reached AI Spot");
            timer += Time.deltaTime;
            if (timer >= Random.Range(waitingTimeMin, waitingTimeMax))
            {
                currentValue = 1;
                timer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AIController AI = other.GetComponent<AIController>();

        if (AI != null)
        {
            currentValue = 0;
        }
    }

}

