using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                var registeredConnections = new List<Vector3>();
                registeredConnections.AddRange(child.RegisteredConnections);
                foreach(var connection in child.Connections)
                {
                    if (registeredConnections.Contains(connection.transform.position))
                    {
                        Gizmos.color = new Color(1,0.4f,0.2f);
                        registeredConnections.Remove(connection.transform.position);
                    }
                    else
                        Gizmos.color = Color.green;
                    
                    Gizmos.DrawLine(child.transform.position, connection.transform.position);
                }

                foreach(var registeredConnection in registeredConnections)
                {
                    Gizmos.color = new Color(1, 0.4f, 0.2f);
                    Gizmos.DrawLine(child.transform.position, registeredConnection);
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
