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

    private bool plHit;
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
        plHit = false;
    }


    void Update()
    {

        if (plHit)
        {
            return;
        }


        playerPosition = pl.position - transform.position;
        Ray ray = new Ray(transform.position, playerPosition);
        RaycastHit2D hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);

        Vector2 direction = (Vector2) (pl.position - gameObject.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the angle to the sprite's rotation (assuming it's a 2D sprite)
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        Vector2 movePosition = direction * speed * Time.deltaTime;


        hit = Physics2D.Raycast(transform.position, (playerPosition).normalized, 100f, layerMask);
        if (hit)
        {
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

        if (!isSeen) return;
        rb.MovePosition(rb.position + movePosition);
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
            isSeen = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            plHit = true;

            StartCoroutine(hitWait());
        }
    }

    IEnumerator hitWait()
    {
        yield return new WaitForSeconds(4);
        rb.constraints = RigidbodyConstraints2D.None;
        plHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Melee"))
        {
            this.Sucked();
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
