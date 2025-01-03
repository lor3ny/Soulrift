using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionActivator : MonoBehaviour
{

    [HideInInspector]
    public Room myroom;

    public Room.Location location;

    private void Awake()
    {
        myroom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Godo");
        if (collision.CompareTag("Connection"))
        {
            myroom.locations.Remove(location);

            Room.Location hitLocation = collision.GetComponent<ConnectionActivator>().location;
            collision.GetComponent<ConnectionActivator>().myroom.locations.Remove(hitLocation);
        }
    }
}
