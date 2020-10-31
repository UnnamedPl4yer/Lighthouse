using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] FloatReference musicVolume;
    [SerializeField] FloatReference soundVolume;
    [SerializeField] FloatReference musicTime;
    private AudioSource musicSource;
    private AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = gameObject.GetComponents<AudioSource>()[0];
        soundSource = gameObject.GetComponents<AudioSource>()[1];
        musicSource.volume = musicVolume.value;
        soundSource.volume = soundVolume.value;
        musicSource.time = musicTime.value;
        musicSource.Play();
    }

    public void Update()
    {
        musicSource.volume = musicVolume.value;
    }
}   
