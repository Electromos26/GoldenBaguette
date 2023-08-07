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

    private PlayerController player;

    private GameObject cubeChild;

    // Start is called before the first frame update
    void Start()
    {
        checkTrap = isTrap;
        timer = 0;
        player = GameObject.FindObjectOfType<PlayerController>();

        cubeChild = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isAlive)
        {
            cubeChild.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkTrap && other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer > timeLimit)
            {
                cubeChild.SetActive(false);
                timer = 0;
            }

        }
    }

}
