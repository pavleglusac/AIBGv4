using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Algorithms
{
    public static List<Pillar> FindPath(Board board, Pillar start, Pillar end)
    {
        Queue<Pillar> queue = new Queue<Pillar>();
        HashSet<Pillar> visited = new HashSet<Pillar>();
        Dictionary<Pillar, Pillar> cameFrom = new Dictionary<Pillar, Pillar>();


        if (start.X != end.X && start.Z != end.Z) {
            return null;
        }

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Pillar current = queue.Dequeue();
            if (current == end)
            {
                break;
            }

            foreach (Pillar next in board.getNeighbours(current))
            {
                if (!visited.Contains(next))
                {
                    queue.Enqueue(next);
                    visited.Add(next);
                    cameFrom[next] = current;
                }
            }
        }

        List<Pillar> path = new List<Pillar>();
        Pillar at = end;
        while (at.X != start.X || at.Z != start.Z)
        {
            if (!at.CanStep()) {
                path.Clear();
                break;
            }
            path.Add(at);
            at = cameFrom[at];
        }
        path.Reverse();

        return path;
    }
}
