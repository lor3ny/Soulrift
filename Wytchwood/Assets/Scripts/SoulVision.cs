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

    private void Update()
    {
         

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if (isSoul)
            {
                // Nothing to do
                spriteEnemy.SetActive(false);
                spriteSoul.SetActive(true);
            }
            else
            {
                // Modify status
                spriteEnemy.SetActive(true);
                spriteSoul.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            spriteEnemy.SetActive(true);
            spriteSoul.SetActive(false);
        }

    }

}
