using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;


    private bool isSeen = false;
    private Vector3 playerPosition;
    private Transform pl;
    private PlayerManager playerManager;
    private Rigidbody2D rb;
    private int layerMask;
    private Vector3 startPosition;

    public SoulVision vision;

    private bool attached;
    private Vector2 attachPos;


    void Start()
    {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        vision = GetComponent<SoulVision>();
        rb = GetComponent<Rigidbody2D>();
        string[] layers = { "Enemy_Soul" };
        layerMask = ~LayerMask.GetMask(layers);
        startPosition = transform.position;
        attached = false;
    }


    void Update()
    {

        if (attached)
        {
            transform.position = pl.position - (Vector3) attachPos;
            return;
        }


        playerPosition = pl.position - transform.position;
        Ray ray = new Ray(transform.position, playerPosition);
        RaycastHit2D hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);

        hit = Physics2D.Raycast(transform.position, (playerPosition).normalized, 100f, layerMask);
        if (hit)
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                // Player is hided! Don't follow.
                isSeen = true;
                Debug.Log("I see you!");
            }
            else
            {
                isSeen = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isSeen) return;

        rb.AddForce(playerPosition.normalized * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerPosition = pl.position - transform.position;
            attachPos = playerPosition;
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Static;
            playerManager.AttachedUp();
            isSeen = false;
            attached = true;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
        if (vision.isSoul)
        {
            playerManager.SoulHitsUp();
        } else
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



}
