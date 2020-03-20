using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;

public static class AStar
{
    public class Link
    {
        public Node a;
        public Node b;

        public float dist;
    }

    public class Node
    {
        public Vector3 Position;
        public float hCost;
        public float gCost;
        public Node previous;
        public bool seen;
        public bool visited;
    }

    public static void BakePath(List<Node> nodes, List<Link> links, Node start, Node target)
    {
        if (start == null || target == null) return;

        //mark all nodes with their distance to the target
        foreach(var node in nodes)
        {
            node.hCost = Vector3.Distance(node.Position, target.Position);
            node.gCost = float.MaxValue / 2f;
            node.previous = null;
            node.visited = false;
        }

        var current = start;

        var toVisit = new Queue<Node>();

        current.gCost = 0f;

        //bfs the nodes, marking the distance traveled and the previous traversal node
        //stop when you get to the target node
        while (current != target)
        {
            var connected = _getConnectedLinks(current, links);
            //checkCosts
            foreach(var link in connected)
            {
                var other = link.a == current ? link.b : link.a;

                var gCost = current.gCost + Vector3.Distance(current.Position, other.Position);

                if(other.gCost > gCost)
                {
                    other.gCost = gCost;
                    other.previous = current;
                }
            }
            //sort and enqueue
            foreach (var link in connected.OrderBy(x => x.a == current ? 
                (x.b.gCost + x.b.hCost) : 
                (x.a.gCost + x.a.hCost)))
            {
                var other = link.a == current ? link.b : link.a;
                toVisit.Enqueue(other);
            }

            current.visited = true;
            
            if(toVisit.Count == 0)
            {
                break;
            }

            current = toVisit.Dequeue();
        }
    }

    public static List<Vector3> GetPath(Node target)
    {
        var toReturn = new List<Vector3>();

        do
        {
            toReturn.Add(target.Position);
            target = target.previous;
        }
        while (target != null);
        
        toReturn.Reverse();
        
        return toReturn;
    }

    private static List<Link> _getConnectedLinks(Node current, List<Link> links)
    {
        var bitA = links.Where(x => x.a == current || x.b == current).ToList();
        var bitB = bitA.Where(x => (x.a == current && !x.b.visited) || (x.b == current && !x.a.visited)).ToList();
        return bitB;
    }
}