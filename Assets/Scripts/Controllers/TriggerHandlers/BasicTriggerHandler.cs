using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BasicTriggerHandler : MonoBehaviour, ITriggerHandler
{
    public Action<Collider> IsTriggered { get; set; }
    public Action<Collider> IsUnTriggered { get; set; }

    public float TriggerWidth = 1f;
    public float TriggerHeight = 1f;
    public float TriggerDepth = 1f;
    public float TriggerX = 0f;
    public float TriggerY = 0f;
    public float TriggerZ = 0f;

    public string[] Mask = new string[] { "Player", "Movers" };

    private Collider[] colliders;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + new Vector3(TriggerX, TriggerY, TriggerZ), new Vector3(TriggerWidth, TriggerHeight, TriggerDepth));
    }

    void FixedUpdate()
    {
        var newColliders = Physics.OverlapBox(
            transform.position + new Vector3(TriggerX, TriggerY, TriggerZ),
            new Vector3(TriggerWidth, TriggerHeight, TriggerDepth),
            Quaternion.identity,
            LayerMask.GetMask(Mask)).Where( x => x.GetComponent<IHouseTaker>() != null);//TODO: put this in a house taker specific trigger settings

        var colliderCount = colliders == null ? 0 : colliders.Length;

        if (colliderCount != 0)
            foreach (var collider in colliders)
            {
                if (newColliders.Contains(collider)) continue;

                IsUnTriggered?.Invoke(collider);
            }

        foreach(var collider in newColliders)
        {
            if (colliders.Contains(collider)) continue;

            IsTriggered?.Invoke(collider);
        }

        colliders = newColliders.ToArray();
    }
}