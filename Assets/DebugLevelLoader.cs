using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLevelLoader : MonoBehaviour
{
    [SerializeField] string levelToLoad = "Level6_Optional3";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
