using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooting : MonoBehaviour
{
    
    private Vector2 mousePos;
    private Vector2 mouseWorldPos;

    public Sprite crosshair;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDelay;
    public GameObject gun;

    private AudioSource source;
    public AudioClip clip;

    private bool hasShot = false;

    public Animator anim;


    private void Start()
    {
        source = GetComponent<AudioSource>();
    }


    void Update()
    {

        mousePos = (Vector2) Input.mousePosition;
        mouseWorldPos = (Vector2) Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        Vector2 direction = (mouseWorldPos - (Vector2)gameObject.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetKeyDown(KeyCode.Mouse0) && hasShot == false)
        {
            GameObject bull = Instantiate(bullet);
            bull.GetComponent<BulletManager>().Initialize(false);
            bull.transform.position = gun.transform.position + (Vector3) direction*2;
            bull.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            source.PlayOneShot(clip, 0.2f);
            anim.SetTrigger("Shoot");
            hasShot = true;
            StartCoroutine(shootDelay());
        }

    }


    IEnumerator shootDelay()
    {   
        yield return new WaitForSeconds(bulletDelay);
        hasShot = false;
    }
}
