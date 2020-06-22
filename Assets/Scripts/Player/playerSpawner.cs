using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab = default;
    bool flipped;

    private void Start()
    {
        flipped = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().startFlipped;
        GameEvents.current.respawnPlayer += SpawnPlayer;
    }
    private void SpawnPlayer()
    {
        Vector3 spawnPoint = transform.position;
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = spawnPoint;
        if (flipped)
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));


        Debug.Log("TODO: Reset pickups");
        ResetCoins();

    }

    void ResetCoins()
    {
        GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");

        for (int i = 0; i < allCoins.Length; i++)
        {
            allCoins[i].GetComponent<pickup>().CoinRestart();
        }
    }
}
