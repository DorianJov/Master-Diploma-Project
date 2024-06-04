using UnityEngine;
using System.Diagnostics;
using System.IO;

public class RestartApplication : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Restart();
        }
    }

    void Restart()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        string exePath = Application.dataPath.Replace("_Data", ".exe");

        // Start a new instance of the application
        Process.Start(exePath);

        // Quit the current application
        Application.Quit();
#endif
    }
}
