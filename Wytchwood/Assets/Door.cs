using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Room;

public class Door : MonoBehaviour
{
    public Room room;
    public Vector2 position;
    private Location location;

    public Location GetLocation()
    {
        return location;
    }
}
