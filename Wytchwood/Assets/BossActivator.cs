using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossManager boss = GetComponentInParent<BossManager>();
            boss.InitializeBoss();
            gameObject.SetActive(false);
        }
    }
}
