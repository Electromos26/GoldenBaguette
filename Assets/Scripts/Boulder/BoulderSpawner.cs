using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Compilation;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    private float respawnTimer;

    [SerializeField]
    private float maxRespawnInterval;

    [SerializeField]
    private float minRespawnInterval;
    
    private Vector3 startPosition;
    private Quaternion startRotation;

    
    public GameObject boulder;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        Instantiate(boulder, startPosition, startRotation);
    }




    private void Update() //Update to destroy boulder after a few seconds
    {

        respawnTimer += Time.deltaTime;

        float respawnInterval = Random.Range(minRespawnInterval, maxRespawnInterval);

        if (respawnTimer > respawnInterval) //Respawn the boulder if the respawn time has passed
        {
            Instantiate(boulder, startPosition, startRotation);
            respawnTimer = 0;
        }

    }

}