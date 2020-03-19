using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HouseDisplayGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        Gizmos.DrawRay(new Ray(transform.position, transform.forward));
    }
}
