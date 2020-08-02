using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class M_HeartMouseover : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void Start()
    {
        text.CrossFadeAlpha(0f, 2f, true);
    }

    private void OnMouseEnter()
    {
        text.CrossFadeAlpha(1f, 0.3f, true);
    }
    private void OnMouseExit()
    {
        text.CrossFadeAlpha(0f, 0.25f, true);
    }
}
