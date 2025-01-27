using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntrance : MonoBehaviour
{
    [HideInInspector]
    public DoorManager door;
    [HideInInspector]
    public bool firstTime = true;
    [HideInInspector]
    public bool isSolved = false;


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

        if (collision.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }
}
