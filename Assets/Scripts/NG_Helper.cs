using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NG_Helper : MonoBehaviour
{
    public io.newgrounds.core ngio_core;
    bool loggedIn;

    //singleton
    public static NG_Helper instance;
    void Awake()
    {
        if (instance == null)
        { instance = this; DontDestroyOnLoad(this.gameObject); }
        else
        { Destroy(this.gameObject); }
    }

    void Start()
    {
        ngio_core.checkLogin((bool logged_in) =>
        {
            if (logged_in)
            {
                onLoggedIn();
            }
            else
            {
                Debug.Log("Player logged in");
            }
        });
    }

    void onLoggedIn()
    {
        io.newgrounds.objects.user player = ngio_core.current_user;
        loggedIn = true;
    }

    public void NGunlockMedal(int medal_id)
    {
        if (loggedIn)
        {
            //create the componenent
            io.newgrounds.components.Medal.unlock medal_unlock = new io.newgrounds.components.Medal.unlock();

            //set req. parameters
            medal_unlock.id = medal_id;

            //call the component on the server and tell it to unlock medal when its done
            medal_unlock.callWith(ngio_core);
            Debug.Log("Sent a message to the server to unlock medal with id " + medal_id);
        }
    }

    public void NGSubmitScore(int score_id, int score)
    {
        if (loggedIn)
        {
            io.newgrounds.components.ScoreBoard.postScore submit_score = new io.newgrounds.components.ScoreBoard.postScore();

            submit_score.id = score_id;
            submit_score.value = score;

            submit_score.callWith(ngio_core);
            Debug.Log("Send a score to the server on the score id" + score_id);
        }

    }
}
