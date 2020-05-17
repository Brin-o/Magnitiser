using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab = default;

    private void Start()
    {
        GameEvents.current.respawnPlayer += SpawnPlayer;
    }
    private void SpawnPlayer()
    {
        Vector3 spawnPoint = transform.position;
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = spawnPoint;
        Debug.Log("TODO: Reset pickups");
    }
}
