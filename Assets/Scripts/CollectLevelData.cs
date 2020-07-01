using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectLevelData : MonoBehaviour
{
    public static CollectLevelData instance;

    public string currentLevel = "0";
    public string prevLevel = "0";
    public string levelSet = "0 -> 0";

    public int deathNum = 0;
    public string coinData;
    public int nextLvl;

    private void Awake()
    {
        if (instance == null)
        { instance = this; DontDestroyOnLoad(this.gameObject); }
        else
        { Destroy(this.gameObject); }
    }

    public void GrabLevels()
    {
        string curLvlName = SceneManager.GetActiveScene().name;

        if (currentLevel == null)
            currentLevel = curLvlName;
        else
        {
            prevLevel = currentLevel;
            currentLevel = curLvlName;
        }

        if (currentLevel != null && prevLevel != null)
        {
            levelSet = prevLevel + " -> " + currentLevel;
        }
    }

    public void GrabNextLvl()
    {
        nextLvl = SceneManager.GetActiveScene().buildIndex + 1;
    }
}
