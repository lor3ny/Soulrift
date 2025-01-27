using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    public bool isForPlayer = true;

    private PlayerManager pl;
    private bool initialized = false;

    public AudioClip enemyClip;
    public AudioClip playerClip;

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
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().PlayOneShot(playerClip, 1.2f);
                StartCoroutine(destroyDelay());
            }
        } else
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Soul"))
            {
                collision.GetComponent<EnemyManager>().DecreaseLife();
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().PlayOneShot(enemyClip, 0.3f);
                StartCoroutine(destroyDelay());
            }

            if (collision.CompareTag("Boss"))
            {
                collision.GetComponent<BossLife>().DecreaseLife(1);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().PlayOneShot(enemyClip, 0.2f);
                StartCoroutine(destroyDelay());
            }
        }
    }


    IEnumerator destroyDelay()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }
}
