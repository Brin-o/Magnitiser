using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Pause : MonoBehaviour
{
    private void OnEnable()
    {
        SlowTime();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        else if (Input.GetKeyDown(KeyCode.M))
            MainMenu();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Resume();

    }

    public void MainMenu()
    {
        NormalTime();
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        NormalTime();
        gameObject.SetActive(false);
    }

    void NormalTime()
    {
        Time.timeScale = 1f;
    }
    void SlowTime()
    {
        Time.timeScale = 0.000001f;
    }
    void Restart()
    {
        Resume();
        if (SpeedrunModule.instance != null)
        {
            SceneManager.LoadScene(1);
            SpeedrunModule.instance.ResetModule();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
