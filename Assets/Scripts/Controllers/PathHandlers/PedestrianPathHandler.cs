using UnityEngine;
using System;
using System.Collections.Generic;

public class PedestrianPathHandler : MonoBehaviour, IPathHandler
{

    private PathingController pathing;

    public bool PathingReady
    {
        get
        {
            return pathing != null && pathing.IsReady;
        }
    }

    public Vector3? RandomPoint 
    {   
        get 
        {
            return pathing == null ? (Vector3?)null : pathing.RandomPoint();
        }
    }

    void Start()
    {
        pathing = WorldGenerator.Pathing;
        pathing.RegisterPathHandler(this);
    }

    public List<Vector3> GetPath(Vector3 location)
    {
        if (pathing == null) return null;

        return pathing.GetPath(transform.position, location);
    }

    public void RegisterPathfinder(PathingController pathingController)
    {
        pathing = pathingController;
    }
}