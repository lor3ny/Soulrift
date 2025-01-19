using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    private Collider2D collider;
    public TriggerEntrance entrance;
    public Room room;
    private Animator animator;
    public AudioClip clip;


    private void Start()
    {
        collider = GetComponent<Collider2D>();
        entrance = GetComponentInChildren<TriggerEntrance>();
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (room.CheckSolved() || entrance.firstTime)
        {
            collider.enabled = false;
            animator.SetTrigger("Open");
            GetComponent<AudioSource>().PlayOneShot(clip, 0.3f);
        }
    }
    public void CloseDoor()
    {
        collider.enabled = true;
        animator.SetTrigger("Close");
    }

    public void ActivateRoom()
    {
        room.Activate();
    }
}
