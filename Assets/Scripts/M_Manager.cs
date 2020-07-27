using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_Manager : MonoBehaviour
{
    [SerializeField] GameObject normalModul = null;
    [Range(0.25f, 1f)] [SerializeField] float inTimer = 0.3f;
    [SerializeField] Transform anchorActive = null;
    [SerializeField] Transform anchorInactive = null;
    [SerializeField] Transform blackout = null;

    [Header("Modules")]
    [SerializeField] Transform module_speedrun = null;
    [Header("Buttons")]
    [SerializeField] Transform itchButton = null;
    [SerializeField] Transform blogButton = null;


    private void Start()
    {
        //destroy modules if they exist
        if (SpeedrunModule.instance != null)
            Destroy(SpeedrunModule.instance.gameObject);
        if (NormalModule.instance != null)
            Destroy(NormalModule.instance.gameObject);
    }

    public void SpeedrunActivate()
    {
        GrabModule(module_speedrun);
    }

    public void StartGame(bool normal)
    {
        blackout.DOMove(Vector3.zero, 0.4f, false);
        Invoke("LoadLevel", 0.45f);
        if (normal == true)
            Instantiate(normalModul);
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
    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/Brin_Design");
        BopButton(blogButton);
    }
    public void OpenHighScores()
    {
        Application.OpenURL("https://docs.google.com/spreadsheets/d/19xpmX-VBPAMhxbAsU_FhWAQwbS_Ra0cMqzAov1VA1OI/edit?usp=sharing");
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
