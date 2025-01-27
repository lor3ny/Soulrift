using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TurretManager : MonoBehaviour
{

    public GameObject bullet;
    private bool hasShot = false;
    private AudioSource source;
    public AudioClip clip;
    public float bulletDelay;
    public float bulletSpeed;

    public bool rotating;

    public Transform[] bulletPoints;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {

        if (hasShot)
            return;

        for(int i = 0; i < bulletPoints.Length; i++)
        {
            GameObject bull = Instantiate(bullet);
            bull.GetComponent<BulletManager>().Initialize(true);
            bull.GetComponent<SpriteRenderer>().color = Color.red;
            bull.transform.position = gameObject.transform.position;
            bull.GetComponent<Rigidbody2D>().velocity = (bulletPoints[i].position - gameObject.transform.position).normalized * bulletSpeed;
            animator.SetTrigger("Shoot");
            //source.PlayOneShot(clip);
        }
      
        

        hasShot = true;
        StartCoroutine(shootDelay());
    }

    IEnumerator shootDelay()
    {
        yield return new WaitForSeconds(bulletDelay);
        hasShot = false;
    }
}
