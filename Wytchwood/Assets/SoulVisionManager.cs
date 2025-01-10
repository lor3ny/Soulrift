using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulVisionManager : MonoBehaviour
{

    private Camera _camera;
    private PlayerManager _player;


    public Color soulVisionUp;
    public Color soulVisionDown;



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
            _camera.backgroundColor = soulVisionUp;
            _player.SpendSouls(1);
        }
    }

    private void DeactivateSoulVision()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _camera.backgroundColor = soulVisionDown;
        }
    }
}
