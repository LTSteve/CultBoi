using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PathingController
{
    private List<IPathHandler> pathHandlers = new List<IPathHandler>();

    private List<AStar.Node> nodes = new List<AStar.Node>();
    private List<AStar.Node> exitNodes = new List<AStar.Node>();

    private List<AStar.Link> links = new List<AStar.Link>();

    public bool IsReady = false;

    public Vector3 RandomPoint()
    {
        return nodes[UnityEngine.Random.Range(0, nodes.Count)].Position;
    }

    public void RegisterPathHandler(IPathHandler entity)
    {
        if(!pathHandlers.Contains(entity))
            pathHandlers.Add(entity);
    }

    private Dictionary<AStar.Node,PedwalkPoint> notification = new Dictionary<AStar.Node,PedwalkPoint>();

    public bool RegisterNode(Vector3 position, bool isExitNode, PedwalkPoint toNotify)
    {
        if (nodes.Any(a => a.Position == position)) return false;

        var toAdd = new AStar.Node
        {
            Position = position
        };

        nodes.Add(toAdd);

        if (isExitNode)
        {
            exitNodes.Add(toAdd);
            notification.Add(toAdd,toNotify);
        }
        return true;
    }

    public bool RegisterPath(Vector3 from, Vector3 to)
    {
        var path = _getPath(from, to);
        if (path != null)
        {
            return false;
        }

        if (from == to) 
            return false;

        var fromNode = nodes.FirstOrDefault(x => x.Position == from);
        if (fromNode == null) fromNode = new AStar.Node { Position = from };

        var toNode = nodes.FirstOrDefault(x => x.Position == to);
        if (toNode == null) toNode = new AStar.Node { Position = to };

        links.Add(new AStar.Link
        {
            a = fromNode,
            b = toNode,
            dist = Vector3.Distance(from, to)
        });

        return true;
    }

    private int nullFiltered = 0;
    private int duplicateFiltered = 0;
    public void LinkExitNodes(List<Vector3> filter)
    {

        Debug.Log("total nodes: " + exitNodes.Count);
        foreach (var node in exitNodes)
        {
            var toLink = _findNearestNode(node.Position, true, filter);

            if (toLink == null)
            {
                nullFiltered++;
                Debug.Log("NullFiltered " + nullFiltered);
                continue;
            }

            if(!RegisterPath(node.Position, toLink.Position))
            {
                duplicateFiltered++;
                Debug.Log("DuplicateFiltered " + duplicateFiltered);
                continue;
            }

            if (notification.ContainsKey(node))
            {
                notification[node].RegisteredConnections.Add(toLink.Position);
            }
            else if (notification.ContainsKey(toLink))
            {
                notification[toLink].RegisteredConnections.Add(node.Position);
            }
        }

        IsReady = true;
    }

    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        var toReturn = new List<Vector3>();

        var start = _findNearestNode(from);
        var target = _findNearestNode(to);

        toReturn.Add(start.Position);

        AStar.BakePath(nodes, links, start, target);

        toReturn.AddRange(AStar.GetPath(target));

        return toReturn;
    }

    private AStar.Link _getPath(Vector3 from, Vector3 to)
    {
        return links.FirstOrDefault(x => (x.a.Position == from && x.b.Position == to) || (x.a.Position == to && x.b.Position == from));
    }

    private AStar.Node _findNearestNode(Vector3 nodePosition, bool exitOnly = false, List<Vector3> filter = null)
    {
        var minDist = float.MaxValue;
        AStar.Node minDistNode = null;

        var nodesToCheck = exitOnly ? exitNodes : nodes;

        var nodeNearest = filter != null ? (
            filter.OrderBy(x => Vector3.Distance(x, nodePosition)).First()
            ) : Vector3.zero;

        foreach (var n in nodesToCheck)
        {
            var dist = Vector3.Distance(nodePosition, n.Position);
            if(dist < minDist)
            {
                if (exitOnly)
                {
                    if (dist == 0) continue;
                    if(filter != null)
                    {

                        var nNearest = filter != null ? (
                            filter.OrderBy(x => Vector3.Distance(x, n.Position)).First()
                        ) : Vector3.zero;

                        //same world tile
                        if(nNearest == nodeNearest)
                        {
                            continue;
                        }

                        //path already exists
                        var existingPath = _getPath(nodePosition, n.Position);
                        if (existingPath != null)
                        {
                            continue;
                        }
                    }
                }

                minDist = dist;
                minDistNode = n;
            }
        }

        return minDistNode;
    }
}