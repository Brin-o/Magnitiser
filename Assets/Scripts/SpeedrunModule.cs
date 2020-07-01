using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Networking;

public class SpeedrunModule : MonoBehaviour
{


    public double timer = 0;
    public string timerString;
    public bool runCompleted = false;

    public bool active = false;
    [SerializeField] string[] safeScenes = null;

    public string username = "Dflt";
    [SerializeField] private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScw0iVac8RriGbTnnyXlQBI1BccNs2uxagiZ3HIr9U58G4fwA/formResponse";


    #region  Singleton
    public static SpeedrunModule instance;
    private void Awake()
    {
        if (instance == null)
        { instance = this; DontDestroyOnLoad(this.gameObject); }
        else
            Destroy(this.gameObject);
    }
    #endregion

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.name == "End")
            SendData();
    }


    private void Update()
    {
        ActivateTimer();
        if (active)
            timer += Time.deltaTime;

        TimeSpan _time = TimeSpan.FromSeconds(timer);

        timerString = string.Format("{0:D2}:{1:D2}.{2:D1}", _time.Minutes, _time.Seconds, _time.Milliseconds);
    }

    private void ActivateTimer()
    {
        //preveri ce smo v sceni, ki ne sme imeti aktivnega timerja. V kolikor smo potem se ustavi
        string _curScene = SceneManager.GetActiveScene().name;
        if (safeScenes.Contains(_curScene))
        {
            active = false;

            if (_curScene == "Menu") //preveri če si v meniju
                timer = 0;

            if (_curScene == "Win")
                SendData();
        }
        else
            active = true;
    }



    //sending highscores to the spreadsheet

    void SendData()
    {
        StartCoroutine(SendHighscore());

        timer = 0;
        username = "Dflt";
    }

    IEnumerator SendHighscore()
    {
        WWWForm _form = new WWWForm();

        _form.AddField("entry.102015260", username);
        _form.AddField("entry.1723966381", timerString);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL, _form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Hiscore upload completed.");
        }
    }
}
