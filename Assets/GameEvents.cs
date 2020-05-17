using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public int pickupNum = 0;
    public int pickups = 0;
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        //pickupNum = pickups.Length;
    }
    public event Action<int> onCollectiblePickup;
    public void CollectibePickedUp(int id)
    {
        if (onCollectiblePickup != null)
        {
            onCollectiblePickup(id);
        }
    }

    public event Action respawnPlayer;
    public void RespawnPlayer()
    {
        if (respawnPlayer != null)
        {
            respawnPlayer();
        }
    }
}
