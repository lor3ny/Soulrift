using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSouls : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().SoulSuckedUp();
            gameObject.SetActive(false);
        }
    }
}
