using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        LoadJsonFromFile("levels");
    }

    void LoadJsonFromFile(string resourceName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(resourceName);
        if (textAsset != null)
        {
            Item root = JsonConvert.DeserializeObject<Item>(textAsset.text);
            Debug.Log("Level data loaded successfully.");

            Levels = new List<ItemData>(root.levels);
        }
        else
        {
            Debug.LogError("Failed to load the level data JSON file from resources.");
        }
    }

    

}
