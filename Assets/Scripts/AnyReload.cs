using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;


public class AnyReload : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
