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
    public bool isBossFight;
    public GameObject spawn;

    public GameObject extraLife;
    public AudioClip baseClip;
    public AudioClip normalClip;
    public AudioClip bossClip;


    [HideInInspector]
    public AudioSource source;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawn.transform.position;
        source = GetComponent<AudioSource>();

        if (isTheBase)
        {
            source.loop = true;
            source.clip = baseClip;
            source.Play();
            return;
        }
        else if (isBossFight)
        {
            source.loop = true;
            source.clip = bossClip;
            source.Play();
            InitializeBossFight();
            return;
        }
        else
        {
            source.loop = true;
            source.clip = normalClip;
            source.Play();
            pg = GetComponent<ProceduralGenerator>();
            pg.StartGenerate();
            return;
        }
    }


    private void InitializeBossFight()
    {

        // COPY THE AMOUNT OF SOULS INSIDE THE LIVES, GENERATE ALSO THE UI
        PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        int soulsCount = player.GetSouls();

        Debug.Log("Start boss fight with: "+soulsCount.ToString()+" souls");

        Vector2 life1 = GameObject.Find("life_1").transform.position;
        life1 = new Vector2(life1.x + 140, life1.y+5);


        for (int i = 0; i<soulsCount; i++)
        {
            Debug.Log(i);
            GameObject newlife = Instantiate(extraLife);
            GameObject livesParent = GameObject.Find("Lives");

            newlife.transform.SetParent(livesParent.transform);

            Vector2 pos = new Vector2(life1.x + 40*i, life1.y);
            newlife.transform.position = pos;
            player.lifeImgs.Add(newlife);
        }

        player.IncreaseLives(soulsCount);
        
        // STARTUP THE BOSS BEHAVIOUR
        BossManager boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
    }
}
