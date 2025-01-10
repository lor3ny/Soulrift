using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private ProceduralGenerator pg;

    public bool isTheBase;

    void Start()
    {

        if (isTheBase)
            return;
        pg = GetComponent<ProceduralGenerator>();
        pg.StartGenerate();
    }

    public void LevelFinished()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
