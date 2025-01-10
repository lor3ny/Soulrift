using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntrance : MonoBehaviour
{
    [HideInInspector]
    public DoorManager door;

    public bool roomActivator;

    [HideInInspector]
    public bool firstTime = true;


    private void Start()
    {
        door = GetComponentInParent<DoorManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            door.OpenDoor();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (firstTime)
            return;

        if (collision.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }
}
