using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] FloatReference musicTime;
    private AudioSource musicSource;

    private void Awake()
    {
        musicSource = GetComponents<AudioSource>()[0];
    }

    public void Play()
    {
        SceneManager.LoadScene("Intro");
    }

    public void SaveMusicTime()
    {
        musicTime.SetValue(musicSource.time);
    }

    public void Pause(bool paused)
    {
        if (paused == true)
        {
            Time.timeScale = 0;
        }

        if (paused == false)
        {
            Time.timeScale = 1;
        }
        
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("EndOfGame");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
