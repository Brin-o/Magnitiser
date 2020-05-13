using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishPoint : MonoBehaviour
{
    int currentLevel;
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
    }
}
