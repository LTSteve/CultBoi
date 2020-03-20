using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PedwalkPoint : MonoBehaviour
{
    public bool exitPoint;

    public List<PedwalkPoint> Connections;

    private void OnDrawGizmos()
    {
        if (exitPoint)
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