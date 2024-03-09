using UnityEngine;
using System.Collections.Generic;

public class EnvConfig : MonoBehaviour
{
    [SerializeField] private TextAsset envFile;

    public static TextAsset EnvFile
    {
        get => Instance.envFile;
        set => Instance.envFile = value;
    }

    private static EnvConfig Instance;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        if (Instance == null)
        {
            Instance = this;
            ReadEnvFile();
        }

    }

    void ReadEnvFile()
    {

        if (EnvFile != null)
        {
            string[] lines = EnvFile.text.Split('\n');

            foreach (string line in lines)
            {
                string[] keyValue = line.Split('=');

                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    PlayerPrefs.SetString(key, value);
                
                }
            }

            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError(".env file not found!");
        }
    }
}
