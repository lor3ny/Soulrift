using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    public float startupDelay;
    public bool isDead;
    public int delay;
    public float rotationSpeed;
    public List<Transform> jumps;


    private BossShooting shooting;
    private BossLife life;
    private bool rotate;

    public GameObject sprite;

    public void InitializeBoss()
    {
        isDead = false;
        rotate = false;
        shooting = GetComponent<BossShooting>();
        life = GetComponent<BossLife>();

        StartCoroutine(Fighting());

    }

    void Update()
    {
        if (rotate)
        {
            sprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator Fighting()
    {
        yield return new WaitForSeconds(startupDelay);
        while (!isDead)
        {
            // ROUTINE 1

            Vector2 startingPosition = transform.position;
            for (int i = 0; i < jumps.Count; i++)
            {
                gameObject.transform.position = jumps[i].position;
                shooting.Shoot();
                // SHOOT
                yield return new WaitForSeconds(delay);
            }
            gameObject.transform.position = startingPosition;
            shooting.Shoot();
            yield return new WaitForSeconds(delay);
            

            // ROUTINE 2
            rotate = true;
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < jumps.Count; i++)
            {
                shooting.Shoot();
                yield return new WaitForSeconds(delay/3);
            }
            rotate = false;
        }
    }


    public void DeathRoutine()
    {
        isDead = true;
        StopCoroutine(Fighting());
        gameObject.SetActive(false);
        // Animation disappearing
    }

}
