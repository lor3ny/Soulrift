using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulVision : MonoBehaviour
{

    public Color soulColor;
    public Color enemyColor;

    public bool isSoul; 

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Every object spawn as a soul
        sr.color = soulColor;
    }

    private void Update()
    {
         

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (isSoul)
            {
                // Nothing to do
            }else
            {
                // Modify status
                sr.color = enemyColor;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            sr.color = soulColor;
        }

    }

}
