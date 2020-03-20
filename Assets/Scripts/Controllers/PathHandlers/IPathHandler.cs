using UnityEngine;
using System.Collections.Generic;

public interface IPathHandler
{
    bool PathingReady { get; }
    Vector3? RandomPoint { get; }
    void RegisterPathfinder(PathingController pathingController);

    List<Vector3> GetPath(Vector3 location);
}