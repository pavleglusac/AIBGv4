using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheapCrystal : Crystal
{
    public GameObject Crystal1ParentObject { get; set; }
    public GameObject Crystal1Object { get; set; }



    public Dictionary<string, Tuple<Material, Material>> materials = new Dictionary<string, Tuple<Material, Material>>();

    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("cheap_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_replenish_turns"));

        foreach (Transform child in transform)
        {
            if (child.name == "Rock004") continue;
            if (!child.TryGetComponent<Renderer>(out var rend)) continue;
            materials[child.name] = new Tuple<Material, Material>(rend.materials[0], rend.materials[1]);
        }
        IsEmpty = false;

    }

    // Update is called once per frame
    void Update()
    {
        if ((Game.Instance.TurnCount > TurnInWhichCrystalBecameEmpty + ReplenishTurns) && RemainingMineHits == 0 && TurnInWhichCrystalBecameEmpty != -1)
        {
            IsEmpty = false;
        }
        foreach (Transform child in transform)
        {
            if (child.name == "Rock004") continue;
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
        return int.Parse(PlayerPrefs.GetString("raw_cheap_crystal_weight"));
    }

}
