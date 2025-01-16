using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private PlayerManager playerManager;
    public SoulVision vision;
    public float life;
    private Animator animator;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        vision = GetComponent<SoulVision>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Death()
    {

        // APPLY SOME EFFECTS: PARTICLES AND SOUND

        Destroy(gameObject);
        if (vision.isSoul)
        {
            playerManager.SoulHitsUp();
        }
        else
        {
            playerManager.EnemyHitsUp();
        }
    }

    public void Sucked()
    {
        Destroy(gameObject); // Cambiare
        if (vision.isSoul)
        {
            Destroy(gameObject);
            playerManager.SoulSuckedUp();
        }
        else
        {
            Destroy(gameObject);
            playerManager.EnemySuckedUp();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            life -= 1;
            if (life <= 0)
            {
                Death();
            }
        }
    }

    public void DecreaseLife()
    {

        animator.SetTrigger("Hit");
        life -= 1;
        if (life <= 0)
        {
            Death();
        }
    }
}
