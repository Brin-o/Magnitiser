using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tweenExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void OnMouseDown()
    {
        if (transform.localScale == Vector3.one)
            transform.DOPunchScale(Vector3.one * 0.25f, 0.2f, 5, 1);
    }
}
