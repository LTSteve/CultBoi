using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandTargetingHandler : MonoBehaviour, ITargetingHandler
{
    public float TargetingRadius = 2f;
    public string[] Mask = new string[] { "Movers" };
    public Faction[] TargetFactions = new Faction[] { Faction.Enemy, Faction.Pedestrian };

    private Collider[] activeCollisions;

    public Transform AcquireTarget()
    {
        var movers = activeCollisions;

        var targets = new Dictionary<Faction, Transform>();
        var targeted = 0;

        if(movers != null)
            foreach(var mover in movers)
            {
                if (mover == null) continue;

                var controller = mover.gameObject.GetComponent<Controller>();

                if(controller != null)
                {
                    if(TargetFactions.Contains(controller.Faction) && !targets.ContainsKey(controller.Faction))
                    {
                        targets.Add(controller.Faction, mover.transform);
                        targeted++;

                        if (targeted >= TargetFactions.Length) break;
                    }
                }
            }

        Transform newTarget = null;

        if(targets.Count > 0)
            foreach(var faction in TargetFactions)
            {
                if (targets.ContainsKey(faction))
                {
                    newTarget = targets[faction];
                    break;
                }
            }

        return newTarget;
    }
    void FixedUpdate()
    {
        activeCollisions = Physics.OverlapSphere(transform.position, TargetingRadius, LayerMask.GetMask(Mask));
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, TargetingRadius);
    }
}