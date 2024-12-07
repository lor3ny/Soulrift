using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProceduralGenerator;

public class Room : MonoBehaviour
{

    public enum Location
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField]
    private List<Location> locations;

    public GameObject UP;
    public GameObject DOWN;
    public GameObject LEFT;
    public GameObject RIGHT;

    public List<Location> GetLocations()
    {
        return locations;
    }
}
