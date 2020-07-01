using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishPoint : MonoBehaviour
{
    int currentLevel;
    [SerializeField] Animator transitionAnimator = null;
    [SerializeField] GameObject transitionEngine = null;

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (transitionEngine != null)
            transitionEngine.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transitionAnimator.SetTrigger("start");
        }
    }

    public void LoadNextLevel()
    {
        if (CollectLevelData.instance != null)
        {
            CollectLevelData data = CollectLevelData.instance;

            data.GrabLevels();
            data.GrabNextLvl();
            data.coinData = CoinEvaluation();

            if (data.currentLevel == "0" || data.prevLevel == "0")
            {
                Debug.LogWarning("Don't have all the data needed, going to the next level");
                SceneManager.LoadScene(currentLevel + 1);
            }

            else
            {
                //loadaj v feedback sceno
                Debug.LogWarning("Going into feedback scene");
                SceneManager.LoadScene("Feedback");
            }
        }
        else
        {
            Debug.LogWarning("No data collection object!");
            SceneManager.LoadScene(currentLevel + 1);
        }
        //{ SceneManager.LoadScene(currentLevel + 1); Debug.LogWarning("No feedback collection"); }
    }

    string CoinEvaluation()
    {
        GameObject[] _allCoins = GameObject.FindGameObjectsWithTag("Coin");

        int _maxCoins = _allCoins.Length;
        int _pickedCoins = 0;

        for (int i = 0; i < _allCoins.Length; i++)
        {
            if (_allCoins[i].GetComponent<pickup>().pickedUp)
                _pickedCoins++;
        }
        string result = "Picked up: " + _pickedCoins + "/" + _maxCoins;
        Debug.Log(result);
        return result;
    }
}
