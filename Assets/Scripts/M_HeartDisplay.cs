using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class M_HeartDisplay : MonoBehaviour
{
    float coinsCollected = 0;
    [SerializeField] float maxCoins = 16;
    [SerializeField] TextMeshProUGUI m_text = null;
    [SerializeField] Color winColor = Color.white;

    void Start()
    {
        coinsCollected = (float)Variables.Saved.Get("collectedCoins");

        m_text.text = coinsCollected + "/" + maxCoins;

        if (coinsCollected == maxCoins)
            GetComponentInChildren<Image>().color = winColor;
    }

}
