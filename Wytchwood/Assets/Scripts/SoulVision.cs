using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulVision : MonoBehaviour
{

    public Color soulColor;
    public Color enemyColor;

    public bool isSoul;

    public GameObject spriteSoul;
    public GameObject spriteEnemy;

    private void Start()
    {

        // Every object spawn as an enemy
        spriteEnemy.SetActive(true);
        spriteSoul.SetActive(false);
    }

    public void ActivateSoul()
    {
        spriteEnemy.SetActive(false);
        spriteSoul.SetActive(true);
    }

    public void DeactivateSoul()
    {
        spriteEnemy.SetActive(true);
        spriteSoul.SetActive(false);
    }

}
