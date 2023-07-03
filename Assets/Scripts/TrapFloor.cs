using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFloor : MonoBehaviour
{
    [SerializeField]
    private bool isTrap;

    private bool checkTrap;

    [SerializeField]
    private float timeLimit;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        checkTrap = isTrap;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkTrap && other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer > timeLimit)
            {
                Destroy(gameObject);
                timer = 0;
            }

        }
    }

}
