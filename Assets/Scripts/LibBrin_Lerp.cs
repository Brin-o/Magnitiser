using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Built based on the Lerp like a pro blog post (https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/?ref=hackernoon.com)
// brin.design 2020

public class LibBrin_Lerp
{

    public static float EaseIn(float a, float b, float t)
    {
        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        return Mathf.Lerp(a, b, t);
    }
    public static float EaseOut(float a, float b, float t)
    {
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        return Mathf.Lerp(a, b, t);
    }

    public static float SmootherStep(float a, float b, float t)
    {
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        return Mathf.Lerp(a, b, t);
    }

}
