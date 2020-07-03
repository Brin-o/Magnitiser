using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpeedrunMenu : MonoBehaviour
{
    [SerializeField] InputField m_input = null;
    [SerializeField] GameObject speedrunPrefab = null;
    [SerializeField] Transform inputField = null;
    [SerializeField] M_Manager menuManager = null;
    [SerializeField] SoundController menuSounds = null;
    string username = "";

    public void InitiateSpeedrun()
    {
        username = m_input.text;

        if (username.Length == 0)
            NoInputError();

        else
        {
            menuSounds.PlayVaried("clickNormal");
            GameObject _module = Instantiate(speedrunPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
            SpeedrunModule _srModule = _module.GetComponent<SpeedrunModule>();

            _srModule.username = username;

            menuManager.StartGame();
        }


    }

    void NoInputError()
    {
        menuSounds.PlayVaried("clickError");
        Debug.Log("No input given");
        if (inputField.localScale == Vector3.one)
            inputField.DOPunchScale(Vector3.one * 0.2f, 0.1f, 3, 1);
        Debug.Log("TODO: Play an error sound");
    }

}
