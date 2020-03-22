using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class CopIntentManager : MonoBehaviour, IIntentManager
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

    public float AggroRange = 10f;
    public float PathfindingFudgeRange = 0.5f;

    //TODO: public float AggroThreshold

    private Transform target;

    private bool aggro = false;

    private ITargetingHandler targeting;

    private IPathHandler pathing;

    private List<Vector3> path = new List<Vector3>();

    void Start()
    {
        targeting = GetComponent<ITargetingHandler>();
        pathing = GetComponent<IPathHandler>();

        var health = GetComponent<IHealthHandler>();
        if(health != null)
        {
            health.Damaged += OnDamaged;
        }
    }

    void OnDamaged(Transform other, float amound)
    {
        CopAlert.Level += 1;
    }

    public void UpdateIntent()
    {
        moveIntent = null;
        moveTarget = null;
        setTarget = false;
        unsetTarget = false;

        if (target != null && Vector3.Distance(transform.position, target.transform.position) > AggroRange)
        {
            target = null;
            unsetTarget = true;
        }

        target = target != null ? target : targeting?.AcquireTarget();
        if (!aggro && target != null)
        {
            var houseTaker = target.GetComponent<IHouseTaker>();
            if (houseTaker != null && houseTaker.TakingHouse)
            {
                CopAlert.Level += 0.2f * Time.deltaTime;
            }
            else
            {
                CopAlert.Level += 0.05f * Time.deltaTime;
            }
        }

        if(CopAlert.Level >= 1f && !aggro && target != null)
        {
            aggro = true;
            setTarget = true;
        }

        if(aggro && target != null)
        {
            moveTarget = target.position;
        }
        else if(pathing != null && pathing.PathingReady)
        {
            if (path.Count > 0 && ((path[0] - transform.position).magnitude < PathfindingFudgeRange))
            {
                path.RemoveAt(0);
            }
            if (path.Count > 0)
            {
                moveTarget = path[0];
            }
            else 
            {
                path = pathing.GetPath(pathing.RandomPoint.Value);
            }
        }

        action1 = aggro;
    }
}