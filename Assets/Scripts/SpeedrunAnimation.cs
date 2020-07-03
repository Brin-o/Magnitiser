using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedrunAnimation : MonoBehaviour
{
    [SerializeField] Transform timer = null;
    [SerializeField] Transform timerAnchor = null;
    [SerializeField] Transform banner = null;
    [SerializeField] Transform bannerAnchor = null;

    private void Start()
    {
        Vector3 timerPos = timer.position;
        timer.position = timerAnchor.position;
        timer.DOMove(timerPos, 0.75f);

        Vector3 bannerPos = banner.position;
        banner.position = bannerAnchor.position;
        banner.DOMove(bannerPos, 1f);
    }
}
