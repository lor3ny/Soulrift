using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulVision : MonoBehaviour
{

    public Color soulColor;
    public Color enemyColor;

    public bool isSoul; 

    public SpriteRenderer sr;

    private void Start()
    {

        // Every object spawn as a soul
        sr.color = enemyColor;
    }

    private void Update()
    {
         

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (isSoul)
            {
                // Nothing to do
                sr.color = soulColor;
            }
            else
            {
                // Modify status
                sr.color = enemyColor;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            sr.color = enemyColor;
        }

    }

}
