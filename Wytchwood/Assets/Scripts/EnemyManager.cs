using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private PlayerManager playerManager;
    public SoulVision vision;
    public float life;
    private Animator animator;

    public AudioClip clipShooter;
    public AudioClip clipBasic;
    public AudioClip clipTurret;

    public bool turret;
    public bool basic;
    public bool shooter;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        vision = GetComponent<SoulVision>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Death()
    {

        // APPLY SOME EFFECTS: PARTICLES AND SOUND

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }

        if (turret)
        {
            GetComponent<AudioSource>().PlayOneShot(clipTurret, 0.4f);
        }
        else if (basic)
        {
            GetComponent<AudioSource>().PlayOneShot(clipBasic, 1.2f);
        } else if (shooter)
        {
            GetComponent<AudioSource>().PlayOneShot(clipShooter, 0.5f);
        }

        if (vision.isSoul)
        {
            playerManager.SoulHitsUp();
        }
        else
        {
            playerManager.EnemyHitsUp();
        }


        StartCoroutine(destroyEnemy());
    }
    IEnumerator destroyEnemy()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
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
