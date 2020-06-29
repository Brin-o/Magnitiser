using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendDataToForm : MonoBehaviour
{
    public string evaluation = "non given";
    public string levelsToEvaluate = "0-0";

    public int deathsNum = 0;
    public string coinData = "0/0";

    [SerializeField] private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSes_2hlUzZfS6B94OHXsb9lgx0OfHFvrqJnWRBUI714N2tKCg/formResponse";

    public void EasierPlus()
    {
        evaluation = "This level was a lot easier";

        TransferData();

        StartCoroutine(Post(evaluation));
        LoadNextLvl();
    }
    public void Easier()
    {
        evaluation = "This level was easier";

        TransferData();

        StartCoroutine(Post(evaluation));
        LoadNextLvl();
    }

    public void Same()
    {
        evaluation = "The difficulty was about the same";

        TransferData();

        StartCoroutine(Post(evaluation));
        LoadNextLvl();
    }
    public void Harder()
    {
        evaluation = "This level was harder";

        TransferData();

        StartCoroutine(Post(evaluation));
        LoadNextLvl();
    }
    public void HarderPlus()
    {
        evaluation = "This level was a lot harder";

        TransferData();

        StartCoroutine(Post(evaluation));
        LoadNextLvl();
    }

    IEnumerator Post(string result)
    {
        WWWForm form = new WWWForm();

        form.AddField("entry.60705062", levelsToEvaluate);
        form.AddField("entry.1653144443", result);
        form.AddField("entry.856417234", deathsNum);
        form.AddField("entry.1907840995", coinData);

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }

    void LoadNextLvl()
    {
        if (CollectLevelData.instance != null)
        {
            CollectLevelData data = CollectLevelData.instance;

            deathsNum = 0;

            SceneManager.LoadScene(data.nextLvl);

        }
        else
        {
            Debug.LogError("No data collection object!");
        }
    }

    void TransferData()
    {
        if (CollectLevelData.instance != null)
        {
            CollectLevelData _data = CollectLevelData.instance;

            levelsToEvaluate = _data.levelSet;
            deathsNum = _data.deathNum;
            coinData = _data.coinData;

            _data.deathNum = 0;

        }
        else
        {
            Debug.LogError("No data collection object!");
        }
    }
}
