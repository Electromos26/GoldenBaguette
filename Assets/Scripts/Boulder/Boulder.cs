using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private PlayerController player;

    private bool playerKilled = false;

    private float destroyTimer;
    [SerializeField]
    private float destroyLimit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.isAlive)
        {
            player.PublicDie();
            playerKilled = true;

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerKilled) //Destroy Boulder after a few seconds that it has killed the player
        {
            destroyTimer += Time.deltaTime;

            if (destroyTimer > destroyLimit)
            {
                destroyTimer = 0;
                playerKilled = false;
                Destroy(this.gameObject);
            }
        }

    }
}
