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
            gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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
                yield return new WaitForSeconds(0.5f);
                gameObject.transform.position = jumps[i].position;
                for(int j = 0; j < 20; j++)
                {
                    shooting.Shoot();
                    yield return new WaitForSeconds(0.1f);
                }
                
            }
            gameObject.transform.position = startingPosition;
            

            // ROUTINE 2
            rotate = true;
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < 20; i++)
            {
                shooting.Shoot();
                yield return new WaitForSeconds(0.3f);
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
