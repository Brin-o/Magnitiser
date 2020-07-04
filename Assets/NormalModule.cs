using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NormalModule : MonoBehaviour
{
    string levelName = "DefaultLevel";
    [SerializeField] TextMeshProUGUI m_text = null, m_textShadow = null;


    #region  Singleton
    public static NormalModule instance;
    private void Awake()
    {
        if (instance == null)
        { instance = this; DontDestroyOnLoad(this.gameObject); }
        else
        { Destroy(SpeedrunModule.instance.gameObject); instance = this; DontDestroyOnLoad(this.gameObject); }
    }
    #endregion

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SpeedrunModule.instance != null)
            SpeedrunModule.instance.gameObject.SetActive(false);
    }


    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.name == "End" || _scene.name == "Menu")
            levelName = "";
        else
        {
            levelName = _scene.name;
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        { m_text.text = levelName; m_textShadow.text = levelName; }
    }

}
