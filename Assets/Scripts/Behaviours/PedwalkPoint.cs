using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PedwalkPoint : MonoBehaviour
{
    public bool exitPoint;

    [HideInInspector]
    public bool Registered;

    public List<PedwalkPoint> Connections;

    [HideInInspector]
    public List<Vector3> RegisteredConnections = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if (Registered)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.6f);
        }
        else if (exitPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.75f);
        }
        else
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}