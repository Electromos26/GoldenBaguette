using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    List <GameObject> allRooms;

    // Start is called before the first frame update
    void Start()
    {
        //Load room 1 and connection
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (GameObject room in allRooms)
        {
            if (room != allRooms[0] && room != allRooms[1])
            room.SetActive(false);
        }
        
    }

    public void EnterRoom(Room loadRoom)
    {
        //check for all of the rooms to load or not load
        foreach (GameObject room in allRooms)
        {
            if (loadRoom.IsNeighbour(room.GetComponent<Room>()))
            {
                room.SetActive(true);
                gameManager.OnCreateInstance();
                //Set AI to Idle

                if (room.GetComponentInChildren<AIController>() != null)
                {
                    foreach(AIController controller in room.GetComponentsInChildren<AIController>())
                    {
                        controller.BackToIdle();
                        Debug.Log("backToidle");
                    }
                }
                if (room.GetComponentInChildren<Boss>() != null)
                {
                    room.GetComponentInChildren<Boss>().BackToIdle();
                }

            }
            else if (room != loadRoom.gameObject)
            {
                room.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
