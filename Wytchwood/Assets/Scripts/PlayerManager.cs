using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private int enemiesHitted;
    private int enemiesSucked;
    private int soulsSucked;
    private int soulsHitted;

    public TMP_Text textVisions;
    public GameObject[] lifeImgs;

    public int maxLives;
    [HideInInspector]
    public int visions;

    private int lives;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<PlayerManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        visions = 0;
        enemiesHitted = 0;
        enemiesSucked = 0;
        soulsSucked = 0;
        soulsHitted = 0;
        lives = maxLives;
        UpdateUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Soul"))
        {
            lives--;
            for(int i = 0; i < maxLives; i++)
            {
                lifeImgs[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < lives; i++)
            {
                lifeImgs[i].SetActive(true);
            }
            if(lives == 0)
            {
                this.Death();
            }
        }
    }

    private void Death()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateUI()
    {
        textVisions.SetText(visions.ToString());
    }

    public void EnemyHitsUp()
    {
        enemiesHitted += 1;
        UpdateUI();
    }

    public void EnemySuckedUp()
    {
        enemiesSucked += 1;
        UpdateUI();
    }

    public void SoulHitsUp()
    {
        soulsHitted += 1;
        if(visions > 0) 
            visions -= 1;
        UpdateUI();
    }

    public void SoulSuckedUp()
    {
        soulsSucked += 1;
        visions += 1;
        UpdateUI();
    }

    public void SpendSouls(int souls)
    {
        if (soulsSucked - souls >= 0)
        {
            enemiesSucked -= souls;
            visions -= souls;
        } else
        {
            Debug.Log("You don't have enough souls!");
        }
        UpdateUI();
    }
}
