using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSouls : MonoBehaviour
{

    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().SoulSuckedUp();
            GetComponent<AudioSource>().PlayOneShot(clip, 0.5f);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
