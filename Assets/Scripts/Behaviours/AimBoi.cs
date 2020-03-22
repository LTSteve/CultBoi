using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBoi : MonoBehaviour
{
    private Quaternion initialRot;

    private void Awake()
    {
        initialRot = transform.rotation;
    }

    public void AimAt(Vector3 position)
    {
        var aimpoint = new Vector3(position.x, transform.position.y, position.z);

        var angle = Vector3.SignedAngle(Vector3.forward, (aimpoint - transform.position), Vector3.up);

        transform.rotation = initialRot * Quaternion.Euler(0, angle,0);
    }
}
