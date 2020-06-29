using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleTile : MonoBehaviour
{
#if (UNITY_EDITOR)

    public bool active = true;
    void Update()
    {
        if (active)
        {
            SpriteRenderer m_sr = GetComponent<SpriteRenderer>();
            CapsuleCollider2D m_capsuleCol = GetComponent<CapsuleCollider2D>();
            BoxCollider2D m_boxCol = GetComponent<BoxCollider2D>();

            Vector2 tiledSize = m_sr.size;

            m_capsuleCol.size = tiledSize;
            m_boxCol.size = tiledSize;
        }

    }
#endif
}
