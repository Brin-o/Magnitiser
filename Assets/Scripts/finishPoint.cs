using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ludiq;
using Bolt;

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
            other.GetComponent<PlayerController>().active = false;
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

            SceneManager.LoadScene(currentLevel + 1);
        }
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

        SendData(_pickedCoins);

        return result;
    }

    void SendData(int coins)//Sends data to the carrier
    {
        GameObject carrier = GameObject.FindGameObjectWithTag("Data");
        CustomEvent.Trigger(carrier, "addCollectedCoin", coins);
    }
}
