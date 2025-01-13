using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    private SpriteRenderer door;
    private Collider2D collider;
    public Room room;


    private void Start()
    {
        door = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }


    public void OpenDoor()
    {
        if (!room.CheckSolved())
            return;

        door.enabled = false;
        collider.enabled = false;
    }

    public void OpenDoor(bool bypass)
    {
        if (bypass)
        {
            door.enabled = false;
            collider.enabled = false;
        }
    }

    public void CloseDoor()
    {
        door.enabled = true;
        collider.enabled = true;
    }

    public void ActivateRoom()
    {
        room.Activate();
    }
}
