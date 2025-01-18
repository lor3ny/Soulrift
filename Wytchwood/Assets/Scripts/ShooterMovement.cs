using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ShooterMovement : MonoBehaviour
{


    private Vector3 playerPosition;
    private Transform pl;
    private bool isSeen = false;
    private int layerMask;
    private Rigidbody2D rb;

    public GameObject bullet;
    private bool hasShot = false;

    public GameObject sprite;

    [SerializeField]
    private float movementSpeed = 1;
    public float bulletSpeed;
    public float bulletDelay;
    public float distanceFromPlayer;
    private AudioSource source;
    public AudioClip clip;

    private Vector2 movePosition;

    // Start is called before the first frame update
    void Start()
    {
        pl = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        string[] layers = { "Enemy_Soul" };
        source = GetComponent<AudioSource>();
        layerMask = ~LayerMask.GetMask(layers);
    }

    void Update()
    {
        // DIRECTION COMPUTATION
        playerPosition = pl.position - transform.position;
        Vector2 direction = (Vector2)playerPosition.normalized;


        // DEBUG
        Ray ray = new Ray(transform.position, playerPosition);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        // DEBUG

        // RAYCAST FOR SHOOTING
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, (playerPosition).normalized, 100f, layerMask);
        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Player is hided! Don't follow.
                isSeen = true;
            }
            else
            {
                isSeen = false;
            }
        }


        // MOVEMENT
        if (direction.x > 0)
        {
            sprite.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.x < 0)
        {
            sprite.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (playerPosition.magnitude > distanceFromPlayer && isSeen)
        {
            movePosition = direction * movementSpeed;
            rb.MovePosition(rb.position + movePosition * Time.deltaTime);
        }

        // SHOOTING
        if (isSeen)
        {
            Shoot();
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movePosition * Time.deltaTime);
    }

    private void Shoot()
    {
        if (hasShot == true)
            return;
         
        Vector2 direction = playerPosition.normalized;
        GameObject bull = Instantiate(bullet);
        bull.GetComponent<BulletManager>().Initialize(true);
        bull.GetComponent<SpriteRenderer>().color = Color.red;
        bull.transform.position = gameObject.transform.position;
        bull.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        source.PlayOneShot(clip, 0.1f);

        hasShot = true;
        StartCoroutine(shootDelay());
    }

    IEnumerator shootDelay()
    {
        yield return new WaitForSeconds(bulletDelay);
        hasShot = false;
    }
}
