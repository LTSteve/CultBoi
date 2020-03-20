using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Pedwalk : MonoBehaviour
{
    private List<PedwalkPoint> points;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(10, 2, 10));

        var children = GetComponentsInChildren<PedwalkPoint>();

        foreach(var child in children)
        {
            if(child.Connections != null && child.Connections.Count > 0)
            {
                foreach(var connection in child.Connections)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(child.transform.position, connection.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //N
        Gizmos.DrawLine(new Vector3(-2, 1, 8), new Vector3(-2, 1, 5));
        Gizmos.DrawLine(new Vector3(-2, 1, 8), new Vector3(2, 1, 5));
        Gizmos.DrawLine(new Vector3(2, 1, 5), new Vector3(2, 1, 8));
    }
}
