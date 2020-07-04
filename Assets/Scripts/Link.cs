using UnityEngine;
using System.Runtime.InteropServices;

public class Link : MonoBehaviour
{

    public string link;

    public void OpenLink()
    {
        Application.OpenURL(link);
    }

    public void OpenLinkJSPlugin()
    {
#if !UNITY_EDITOR
		openWindow(link);
#endif
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

}