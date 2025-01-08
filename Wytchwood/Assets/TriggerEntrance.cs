using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntrance : MonoBehaviour
{

    private DoorManager door;

    public bool roomActivator;
    private bool firstTime = true;


    private void Start()
    {
        door = GetComponentInParent<DoorManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!roomActivator)
            return;  

        if (roomActivator && firstTime)
        {
            door.ActivateRoom();
            firstTime = false;
        }

        if (collision.CompareTag("Player"))
        {
            door.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (roomActivator)
            return;

        if (roomActivator && firstTime)
        {
            door.ActivateRoom();
            firstTime = false;
        }

        if (collision.CompareTag("Player"))
        {
            door.OpenDoor();
        }
    }
}
