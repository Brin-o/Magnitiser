using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class M_ButtonHighlighter : MonoBehaviour
{
    [SerializeField] TMP_FontAsset highlighted;
    [SerializeField] TMP_FontAsset normal;
    TextMeshProUGUI m_text;
    Button m_button;


    private void Start()
    {
        m_button = GetComponent<Button>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        m_button.onClick.AddListener(ChangeToBold);
    }
    void ChangeToBold()
    {
        m_text.font = highlighted;
    }
}
