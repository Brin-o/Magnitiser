using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Bolt;

public class M_LocalHighScore : MonoBehaviour
{

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Global Leaderboards (Your best: " + Variables.Saved.Get("bestTimeString") + " )";
    }
}
