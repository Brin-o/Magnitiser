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

}
