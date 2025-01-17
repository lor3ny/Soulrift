using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectLevel : MonoBehaviour
{

    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            text.SetText("-");
        }
        else
        {
            text.SetText((SceneManager.GetActiveScene().buildIndex - 1).ToString());
        }
    }
}
