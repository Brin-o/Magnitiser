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
            CoinEvaluation();
            transitionAnimator.SetTrigger("start");
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel + 1);
    }

    void CoinEvaluation()
    {
        GameObject[] _allCoins = GameObject.FindGameObjectsWithTag("Coin");
        int _maxCoins = _allCoins.Length;
        int _pickedCoins = 0;

        for (int i = 0; i < _allCoins.Length; i++)
        {
            if (_allCoins[i].GetComponent<pickup>().pickedUp)
                _pickedCoins++;
        }

        Debug.Log("Picked up: " + _pickedCoins + "/" + _maxCoins);
    }
}
