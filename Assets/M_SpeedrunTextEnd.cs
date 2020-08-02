using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_SpeedrunTextEnd : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText = null;
    void Awake()
    {
        scoreText.text = SpeedrunModule.instance.timerString;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
