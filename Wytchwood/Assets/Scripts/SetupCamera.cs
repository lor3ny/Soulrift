using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCamera : MonoBehaviour
{


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<SetupCamera>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        CinemachineVirtualCamera cam = GetComponent<CinemachineVirtualCamera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cam.Follow = player.transform;
    }
}
