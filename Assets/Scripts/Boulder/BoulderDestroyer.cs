using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDestroyer : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) //Destroy the boulder when it collides with a specific wall
    {
        if (other.CompareTag("Boulder"))
        {
            Destroy(other.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
