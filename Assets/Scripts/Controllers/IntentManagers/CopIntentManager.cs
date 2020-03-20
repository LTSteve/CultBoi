using UnityEngine;
using System.Linq;

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

    //TODO: public float AggroThreshold

    private Transform target;

    private bool aggro = false;

    private ITargetingHandler targeting;


    void Start()
    {
        targeting = GetComponent<ITargetingHandler>();
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
            aggro = true;
            setTarget = true;
        }

        if(aggro && target != null)
        {
            moveTarget = target.position;
        }

        action1 = aggro;
    }
}