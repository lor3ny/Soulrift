using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject bullet;
    private AudioSource source;
    public AudioClip clip;
    public float bulletSpeed;

    public Transform[] bulletPoints;


    public void Shoot()
    {
        for (int i = 0; i < bulletPoints.Length; i++)
        {
            GameObject bull = Instantiate(bullet);
            bull.GetComponent<BulletManager>().Initialize(true);
            bull.GetComponent<SpriteRenderer>().color = Color.red;
            bull.transform.position = gameObject.transform.position;
            bull.GetComponent<Rigidbody2D>().velocity = (bulletPoints[i].position - gameObject.transform.position).normalized * bulletSpeed;
            //source.PlayOneShot(clip);
        }
    }
}
