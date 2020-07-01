using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] listOfButtons = null;
    void Start()
    {
        listOfButtons = GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
