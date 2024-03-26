using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public class Board : MonoBehaviour
{
    public Pillar[,] Pillars { get; set; }
    public CheapCrystal[] CheapCrystals { get; set; }
    public ExpensiveCrystal[] ExpensiveCrystals { get; set; }
    public Base[] Bases { get; set; }
    public List<House> Houses { get; set; }

    // Get houses by firstplayerturn
    public int CountPlayersHouses(bool IsFirstPlayers)
    {
        return Houses.Where(house => house.IsFirstPlayers == IsFirstPlayers).ToList().Count;
    }

    public List<Pillar> getNeighbours(Pillar pillar)
    {
        List<Pillar> neighbours = new List<Pillar>();
        int x = pillar.X;
        int z = pillar.Z;

        int w = Pillars.GetLength(0);
        int h = Pillars.GetLength(1);

        if (x > 0) neighbours.Add(Pillars[x - 1, z]);
        if (x < w - 1) neighbours.Add(Pillars[x + 1, z]);
        if (z > 0) neighbours.Add(Pillars[x, z - 1]);
        if (z < h - 1) neighbours.Add(Pillars[x, z + 1]);

        return neighbours;
    }

    public House FindHouse(int x, int z)
    {
        return Houses.Where(house => house.X == x && house.Z == z).FirstOrDefault();
    }




    public string DrawBoard()
    {
        Dictionary<string, string> temp = new Dictionary<string, string>();
        Dictionary<string, string> symbols = new Dictionary<string, string>();
        symbols.Add("Empty", PlayerPrefs.GetString("empty_pillar_symbol"));
        symbols.Add("Player1", PlayerPrefs.GetString("player1_symbol"));
        symbols.Add("Player2", PlayerPrefs.GetString("player2_symbol"));
        symbols.Add("CheapCrystal", PlayerPrefs.GetString("cheep_crystal_symbol"));
        symbols.Add("ExpensiveCrystal", PlayerPrefs.GetString("expensive_crystal_symbol"));
        symbols.Add("BasePlayer1", PlayerPrefs.GetString("player1_castle_symbol"));
        symbols.Add("BasePlayer2", PlayerPrefs.GetString("player2_castle_symbol"));
        symbols.Add("House", PlayerPrefs.GetString("refinement_facility_symbol"));

        foreach (KeyValuePair<string, string> entry in symbols)
        {
            temp[entry.Key] = entry.Value.Replace("\"", "");
        }
        int rows = Pillars.GetLength(0);
        int cols = Pillars.GetLength(1);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                string targetString = temp[Pillars[i, j].PillarState.ToString()];
                sb.Append(targetString);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

}
