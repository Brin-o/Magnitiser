using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab = default;
    public void SpawnPlayer()
    {
        Vector3 spawnPoint = transform.position;
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = spawnPoint;

    }
}
