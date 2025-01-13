using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    public bool isForPlayer = true;

    private PlayerManager pl;
    private bool initialized = false;

    public void Initialize(bool isToPlayer)
    {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        isForPlayer = isToPlayer;
        initialized = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!initialized)
            return;

        if (collision.CompareTag("Room"))
        {
            Destroy(gameObject);
        }


        if (isForPlayer)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerManager>().GeneralHit();
                Destroy(gameObject);
            }
        } else
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Soul"))
            {
                collision.GetComponent<EnemyManager>().Death();
                Destroy(gameObject);
            }
        }
    }
}
