using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpensiveCrystal : Crystal
{
    public GameObject Crystal2ParentObject { get; set; }
    public GameObject Crystal2Object { get; set; }

    public Dictionary<string, Tuple<Material, Material>> materials = new Dictionary<string, Tuple<Material, Material>>();


    // Start is called before the first frame update
    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("expensive_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_replenish_turns"));

        foreach (Transform child in transform)
        {
            if (child.name == "Rock003") continue;
            if (!child.TryGetComponent<Renderer>(out var rend)) continue;
            materials[child.name] = new Tuple<Material, Material>(rend.materials[0], rend.materials[1]);
        }
        IsEmpty = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Rock003") continue;
            if (!child.TryGetComponent<Renderer>(out var rend)) continue;
            Tuple<Material, Material> material = materials[child.name];
            if (IsEmpty)
            {
                rend.materials[0] = material.Item2;
                rend.materials[1] = material.Item2;
                rend.material = material.Item2;
            }
            else
            {
                rend.materials[0] = material.Item1;
                rend.materials[1] = material.Item1;
                rend.material = material.Item1;
            }
        }
    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        // Added so that players can not click 3D objects trough UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!Game.Instance.ArePlayersLanded) return;

        Actions.Mine(X, Z);
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    public static int GetUnprocessedWeight()
    {
        return int.Parse(PlayerPrefs.GetString("raw_expensive_crystal_weight"));
    }

}
