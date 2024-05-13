using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class CustomLogger : MonoBehaviour
{
    private string logFilePath;
    private static bool created = false;

    void ClearFile()
    {
        File.WriteAllText(logFilePath, string.Empty);

    }

    void Awake()
    {
        var directory = Path.Combine(Application.dataPath, "../");
        logFilePath = Path.Combine(directory, "gameLog.txt");

        if (!created && SceneManager.GetActiveScene().name == "Main menu")
        {
            ClearFile();
        }
        created = true;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = string.Format("[{0}] {1}: {2}\n", System.DateTime.Now, type, logString);
        
        File.AppendAllText(logFilePath, logEntry);
    }
}
