using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private int enemiesAttached;
    private int enemiesHitted;
    private int enemiesSucked;
    private int soulsSucked;
    private int soulsHitted;

    public TMP_Text eHit;
    public TMP_Text sHit;
    public TMP_Text eSucked;
    public TMP_Text sSucked;
    public TMP_Text textVisions;

    public int lives;
    [HideInInspector]
    public int visions;


    private void Start()
    {
        visions = 0;
        enemiesAttached = 0;
        enemiesHitted = 0;
        enemiesSucked = 0;
        soulsSucked = 0;
        soulsHitted = 0;
        UpdateTexts();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Souls"))
        {
            lives--;
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

    private void UpdateTexts()
    {
        eHit.SetText("Enemies Hit: " + enemiesHitted.ToString());
        sHit.SetText("Soulds Hit: " + soulsHitted.ToString());
        eSucked.SetText("Enemies Sucked: " + enemiesSucked.ToString());
        sSucked.SetText("Souls Sucked: " + soulsSucked.ToString());
        textVisions.SetText(visions.ToString());
    }


    public void AttachedUp()
    {
        enemiesAttached += 1;
        UpdateTexts();
    }

    public void AttachedDown()
    {
        enemiesAttached -= 1;
        UpdateTexts();
    }

    public void EnemyHitsUp()
    {
        enemiesHitted += 1;
        UpdateTexts();
    }

    public void EnemySuckedUp()
    {
        enemiesSucked += 1;
        UpdateTexts();
    }

    public void SoulHitsUp()
    {
        soulsHitted += 1;
        visions -= 1;
        UpdateTexts();
    }

    public void SoulSuckedUp()
    {
        soulsSucked += 1;
        visions += 1;
        UpdateTexts();
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
        UpdateTexts();
    }
}
