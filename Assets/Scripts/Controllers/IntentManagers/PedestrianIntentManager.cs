using System.Collections.Generic;
using UnityEngine;

public class PedestrianIntentManager : MonoBehaviour, IIntentManager
{
    public Vector2? moveIntent { get; private set; } = Vector2.zero;
    public Vector3? moveTarget { get; set; } = Vector3.zero;
    public bool unsetTarget { get; private set; }
    public bool setTarget { get; private set; }
    public float lookIntent { get; private set; } = 0f;
    public bool action1 { get; private set; } = false;
    public bool action2 { get; private set; } = false;
    public bool action3 { get; private set; } = false;
    public Vector3? mouseLocation { get; private set; } = null;
    public bool Teleport { get; private set; } = true;

    public float Patience = 10f;

    public float DoubletimeModifier = 2f;

    public float PathfindingFudgeRange = 0.5f;

    private IPathHandler pathing;

    private List<Vector3> path = new List<Vector3>();

    public float currentPatience = 10f;

    private static PedestrianIntentManager me = null;

    void Start()
    {
        if (me == null) me = this;
        pathing = GetComponent<IPathHandler>();
    }

    public void UpdateIntent()
    {
        moveIntent = null;
        moveTarget = null;
        setTarget = false;
        unsetTarget = false;
        Teleport = false;

        if (pathing != null && pathing.PathingReady)
        {
            if (path.Count > 0 && ((path[0] - transform.position).magnitude < PathfindingFudgeRange))
            {
                if (me == this) Debug.Log("Moving To Next Leg");
                path.RemoveAt(0);
            }

            if (path.Count > 0)
            {
                moveTarget = path[0];
            }
            else
            {
                if (me == this) Debug.Log("Reached my destination");
                path = pathing.GetPath(pathing.RandomPoint.Value);
            }
        }
    }
}