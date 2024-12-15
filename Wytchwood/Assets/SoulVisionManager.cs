using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulVisionManager : MonoBehaviour
{

    private Camera _camera;


    public Color soulVisionUp;
    public Color soulVisionDown;


    private void Start()
    {
        _camera = Camera.main;
        _camera.backgroundColor = soulVisionDown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _camera.backgroundColor = soulVisionUp;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _camera.backgroundColor = soulVisionDown;
        }
    }
}
