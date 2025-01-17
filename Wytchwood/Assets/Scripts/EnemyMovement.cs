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
    private Rigidbody2D rb;
    private int layerMask;
    private Vector3 startPosition;
    private EnemyManager enemyManager;

    private bool plHit;
    private Vector2 attachPos;

    public GameObject spriteEnemy;
    public GameObject spriteSoul;


    void Start()
    {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyManager = GetComponent<EnemyManager>();
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

        if (direction.x > 0)
        {
            spriteEnemy.GetComponent<SpriteRenderer>().flipX = false;
            spriteSoul.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteEnemy.GetComponent<SpriteRenderer>().flipX = true;
            spriteSoul.GetComponent<SpriteRenderer>().flipX = true;
        }

        Vector2 movePosition = direction * speed * Time.deltaTime;


        hit = Physics2D.Raycast(transform.position, (playerPosition).normalized, 100f, layerMask);
        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isSeen = true;
                Debug.Log("I see you!");
            }
            else
            {
                isSeen = false;
            }
        }

        if (!isSeen) 
            return;
        rb.MovePosition(rb.position + movePosition);
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
            enemyManager.Sucked();
        }
    }
}
