using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private PlayerManager playerManager;
    public SoulVision vision;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        vision = GetComponent<SoulVision>();
    }

    public void Death()
    {
        Destroy(gameObject);
        if (vision.isSoul)
        {
            playerManager.SoulHitsUp();
        }
        else
        {
            playerManager.EnemyHitsUp();
        }
    }

    public void Sucked()
    {
        Destroy(gameObject); // Cambiare
        if (vision.isSoul)
        {
            Destroy(gameObject);
            playerManager.SoulSuckedUp();
        }
        else
        {
            Destroy(gameObject);
            playerManager.EnemySuckedUp();
        }
    }
}
