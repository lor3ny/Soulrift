using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{

    public float maxBossLife;
    public Slider lifeUI;
    private float bossLife;

    private Animator animator;



    private void Start()
    {
        lifeUI.maxValue = maxBossLife;
        lifeUI.value = maxBossLife;
        bossLife = maxBossLife;
        animator = GetComponentInChildren<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Melee"))
        {
            DecreaseLife(2);
        }
    }

    public void DecreaseLife(float value)
    {
        animator.SetTrigger("Hit");
        bossLife -= value;
        if(bossLife <= 0)
        {
            Death();
        }
        UpdateUI();
    }

    private void Death()
    {
        Debug.Log("You Won!");
        GetComponent<BossManager>().DeathRoutine();
    }

    private void UpdateUI()
    {
        lifeUI.value = bossLife;
    }
}
