using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Pillar[,] Pillars {get; set;}
    public Tree[] Trees {get; set;}
    public Rock[] Rocks {get; set;}
    

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
    
}
