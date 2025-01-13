using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private ProceduralGenerator pg;
    private GameObject player;

    public bool isTheBase;
    public GameObject spawn;


    public Animator nextlevelAnimator;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawn.transform.position;
        nextlevelAnimator = GameObject.Find("NextlevelScreen").GetComponent<Animator>();

        if (isTheBase)
            return;

        pg = GetComponent<ProceduralGenerator>();
        pg.StartGenerate();
    }
}
