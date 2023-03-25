using UnityEditor;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void StopApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE
        Application.Quit();
#elif UNITY_WEBGL
        Application.OpenURL(webplayerQuitURL);
#endif
    }
}