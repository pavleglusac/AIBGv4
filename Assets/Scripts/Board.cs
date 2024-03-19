using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public class Board : MonoBehaviour
{
    public Pillar[,] Pillars {get; set;}
    public CheapCrystal[] CheapCrystals {get; set;}
    public ExpensiveCrystal[] ExpensiveCrystals {get; set;}
    public Base[] Bases {get; set;}
    public List<House> Houses { get; set;}
    

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
        int rows = Pillars.GetLength(0);
        int cols = Pillars.GetLength(1);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append(Pillars[i, j].PillarState);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    
}
