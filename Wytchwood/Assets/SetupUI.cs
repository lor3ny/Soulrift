using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupUI : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<SetupUI>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
