using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDecal : MonoBehaviour
{
    SpriteRenderer m_sprite;
    public Color m_color;
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_color.a = 0.7f;
        m_sprite.color = m_color;
        m_sprite.DOFade(0f, 2.9f).SetEase(Ease.InQuad);
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
