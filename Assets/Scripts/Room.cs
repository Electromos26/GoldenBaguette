using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    List<Room> neighbours;
    public bool IsNeighbour(Room potentialNeighbour)
    {
        foreach (Room room in neighbours)
        {
            if (room == potentialNeighbour)
                return true;
        }
        return false;
    }
}
