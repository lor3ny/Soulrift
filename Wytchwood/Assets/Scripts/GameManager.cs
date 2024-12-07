using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private ProceduralGenerator pg;

    void Start()
    {
        pg = GetComponent<ProceduralGenerator>();
        pg.StartGenerate();
    }
}
