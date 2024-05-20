using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class LevelData : MonoBehaviour
{

    public class Item
    {
        public List<ItemData> levels;
    }

    public class ItemData
    {
        public List<List<int>> cheap;
        public List<List<int>> expensive;
    }

    public static List<ItemData> Levels = new();

    
    void Awake()
    {
        LoadJsonFromFile();
    }

    void LoadJsonFromFile()
    {
        string levelsJsonPath = Path.Combine(Application.streamingAssetsPath, "levels.json");
        if ( File.Exists(levelsJsonPath) )
        {
            string json = File.ReadAllText(levelsJsonPath);
            Item root = JsonConvert.DeserializeObject<Item>(json);
            Debug.Log("Level data loaded successfully.");
            Levels = new List<ItemData>(root.levels);
        }
        else
        {
            Debug.LogError("Failed to load the level data JSON file from resources.");
        }
    }

    

}
