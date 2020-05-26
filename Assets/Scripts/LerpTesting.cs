using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTesting : MonoBehaviour
{
    [SerializeField] float lerpTime = 1f;

    [SerializeField] float currentLerpTime;

    [SerializeField] float moveDistance = 10f;

    Vector2 startPos;

    Vector2 endPos;
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.up * moveDistance;
        Debug.Log("Staqrtin at: " + startPos + "ending at pos: " + endPos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }


        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float t = currentLerpTime / lerpTime;
        //Ease In
        //t = Mathf.Sin(t * Mathf.PI * 0.5f);

        //EaseOut
        //t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);

        //Smooth(er) Step - Super EaseIn/EaseOut
        //t = t * t * t * (t * (6f * t - 15f) + 10);


        transform.position = Vector2.Lerp(startPos, endPos, t);
    }
}
