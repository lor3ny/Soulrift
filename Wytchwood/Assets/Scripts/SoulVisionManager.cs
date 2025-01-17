using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulVisionManager : MonoBehaviour
{

    private Camera _camera;
    private PlayerManager _player;


    public Color soulVisionUp;
    public Color soulVisionDown;


    private GameObject[] enemies;
    private GameObject[] souls;



    private void Start()
    {
        _camera = Camera.main;
        _camera.backgroundColor = soulVisionDown;
        _player = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        ActivateSoulVision();
        DeactivateSoulVision();
    }


    private void ActivateSoulVision()
    { 
        if (_player.visions <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _camera = Camera.main;

            souls = GameObject.FindGameObjectsWithTag("Soul");

            _camera.backgroundColor = soulVisionUp;
            _player.SpendSouls(1);

            for (int i = 0; i < souls.Length; i++)
            {
                souls[i].GetComponent<SoulVision>().ActivateSoul();
            }

            Time.timeScale = 0.25f;  
        }
    }

    private void DeactivateSoulVision()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _camera = Camera.main;

            _camera.backgroundColor = soulVisionDown;

            souls = GameObject.FindGameObjectsWithTag("Soul");

            for (int i = 0; i < souls.Length; i++)
            {
                souls[i].GetComponent<SoulVision>().DeactivateSoul();
            }

            Time.timeScale = 1.0f;
        }
    }
}
