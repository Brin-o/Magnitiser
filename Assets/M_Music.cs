using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class M_Music : MonoBehaviour
{
    string currentScene;

    [SerializeField] AudioSource songMenu = null;
    [SerializeField] AudioSource songNormal = null;
    [SerializeField] AudioSource songSpeed = null;
    bool inGame = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SelectSong(SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SelectSong(_scene);
    }

    void SelectSong(Scene _scene)
    {
        if (_scene.name == "Menu")
        {
            songMenu.DOFade(1, 0.5f);
            songMenu.Play();

            songNormal.DOFade(0, 0.5f);
            Invoke("StopNormal", 0.5f);
            inGame = false;
        }
        else if (_scene.name != "Menu" && !inGame)
        {
            inGame = true;
            songMenu.DOFade(0, 0.5f);
            Invoke("StopMenu", 0.5f);

            songNormal.DOFade(1, 0.5f);
            songNormal.Play();
        }
    }

    void StopNormal()
    {
        songNormal.Stop();
    }
    void StopSpeed()
    {
        songSpeed.Stop();
    }
    void StopMenu()
    {
        songMenu.Stop();
    }
}
