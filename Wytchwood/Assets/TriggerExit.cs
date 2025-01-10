using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{

    public TriggerEntrance entrance;



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (entrance.roomActivator && entrance.firstTime)
        {
            entrance.door.ActivateRoom();
            entrance.firstTime = false;
        }

        if (collision.CompareTag("Player"))
        {
            entrance.door.CloseDoor();
            gameObject.SetActive(false);
        }
    }
}
