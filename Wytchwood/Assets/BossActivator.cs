using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{

    public AudioClip bossfight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            //gameManager.source.Stop();
            gameManager.source.clip = bossfight;
            gameManager.source.Play();
            BossManager boss = GetComponentInParent<BossManager>();
            boss.InitializeBoss();
            gameObject.SetActive(false);
        }
    }
}
