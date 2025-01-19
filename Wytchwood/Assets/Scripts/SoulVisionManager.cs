using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoulVisionManager : MonoBehaviour
{

    private Camera _camera;
    private PlayerManager _player;


    public Color soulVisionUp;
    public Color soulVisionDown;


    private GameObject[] enemies;
    private GameObject[] souls;

    private Volume normalVolume;
    private Volume visionVolume;


    private AudioSource AudioSource;
    public AudioClip clip;


    private void Start()
    {
        _camera = Camera.main;
        _camera.backgroundColor = soulVisionDown;
        AudioSource = GetComponent<AudioSource>();
        _player = GetComponent<PlayerManager>();

        normalVolume = GameObject.Find("NormalVolume").GetComponent<Volume>();
        visionVolume = GameObject.Find("VisionVolume").GetComponent<Volume>();

        normalVolume.enabled = true;
        visionVolume.enabled = false;
    }

    private void Update()
    {
        ActivateSoulVision();
    }


    private void ActivateSoulVision()
    { 
        if (_player.visions <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        { 

            _camera = Camera.main;

            souls = GameObject.FindGameObjectsWithTag("Soul");

            normalVolume = GameObject.Find("NormalVolume").GetComponent<Volume>();
            visionVolume = GameObject.Find("VisionVolume").GetComponent<Volume>();

            normalVolume.enabled = false;
            visionVolume.enabled = true;

            _camera.backgroundColor = soulVisionUp;
            _player.SpendSouls(1);

            for (int i = 0; i < souls.Length; i++)
            {
                souls[i].GetComponent<SoulVision>().ActivateSoul();
            }

            AudioSource.loop = true;
            AudioSource.PlayOneShot(clip, 0.5f);

            Time.timeScale = 0.25f;

            StartCoroutine(VisionDuration());
        }
    }

    IEnumerator VisionDuration()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        DeactivateSoulVision();
    }

    private void DeactivateSoulVision()
    {
        _camera = Camera.main;

        normalVolume = GameObject.Find("NormalVolume").GetComponent<Volume>();
        visionVolume = GameObject.Find("VisionVolume").GetComponent<Volume>();

        normalVolume.enabled = true;
        visionVolume.enabled = false;

        _camera.backgroundColor = soulVisionDown;

        souls = GameObject.FindGameObjectsWithTag("Soul");

        for (int i = 0; i < souls.Length; i++)
        {
            souls[i].GetComponent<SoulVision>().DeactivateSoul();
        }

        AudioSource.Stop();
        AudioSource.loop = false;

        Time.timeScale = 1.0f;
    }
}
