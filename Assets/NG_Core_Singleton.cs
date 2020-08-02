using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NG_Core_Singleton : MonoBehaviour
{
    public static NG_Core_Singleton instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
