using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulVision : MonoBehaviour
{

    public Color soulColor;
    public Color enemyColor;

    public bool isSoul;
    public bool notAffect;

    public GameObject spriteSoul;
    public GameObject spriteEnemy;

    private void Start()
    {

        if (notAffect)
            return;

        // Every object spawn as an enemy
        spriteEnemy.SetActive(true);
        spriteSoul.SetActive(false);
    }

    public void ActivateSoul()
    {
        if (notAffect)
            return;

        if (!isSoul)
            return;

        spriteEnemy.SetActive(false);
        spriteSoul.SetActive(true);
    }

    public void DeactivateSoul()
    {
        if (notAffect)
            return;
        
        if (!isSoul)
            return;

        spriteEnemy.SetActive(true);
        spriteSoul.SetActive(false);
    }

}
