using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_Manager : MonoBehaviour
{
    [Range(0.25f, 1f)] [SerializeField] float inTimer = 0.3f;
    [SerializeField] Transform anchorActive = null;
    [SerializeField] Transform anchorInactive = null;
    [SerializeField] Transform blackout = null;

    [Header("Modules")]
    [SerializeField] Transform module_speedrun = null;
    [Header("Buttons")]
    [SerializeField] Transform itchButton = null;
    [SerializeField] Transform blogButton = null;

    public void SpeedrunActivate()
    {
        GrabModule(module_speedrun);
    }

    public void StartGame()
    {
        blackout.DOMove(Vector3.zero, 0.4f, false);
        Invoke("LoadLevel", 0.45f);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }


    public void OpenItch()
    {
        Application.OpenURL("https://brin-o.itch.io/");
        BopButton(itchButton);
    }
    public void OpenBlog()
    {
        Application.OpenURL("https://brin.design/");
        BopButton(blogButton);
    }

    void BopButton(Transform btn)
    {
        if (btn.localScale == Vector3.one)
            btn.DOPunchScale(Vector3.one * 0.2f, 0.175f, 3, 1).SetEase(Ease.OutCubic);
    }

    public void ReturnModule(Transform module)
    {
        module.DOMove(anchorInactive.position, inTimer).SetEase(Ease.InQuad);
    }
    void GrabModule(Transform module)
    {
        module.DOMove(anchorActive.position, inTimer).SetEase(Ease.OutQuad);
    }

}
