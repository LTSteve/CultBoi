using UnityEngine;

public class DemonAttackActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    public float AttackRange = 4f;

    private Transform target;

    private bool acting;

    private ITargetingHandler targeting;

    void Start()
    {
        targeting = GetComponent<ITargetingHandler>();
    }

    public void HandleAction(IIntentManager intent)
    {
        if (targeting == null) return;

        acting = (ActionNumber == 1 && intent.action1) ||
            (ActionNumber == 2 && intent.action2) ||
            (ActionNumber == 3 && intent.action3);

        if (intent.unsetTarget)
        {
            target = null;
        }
        else if(target == null)
        {
            target = targeting.AcquireTarget();
        }

        if (target == null) return;

        if(Vector3.Distance(target.position, transform.position) < AttackRange)
        {
            Debug.Log("ATTACK");
        }
    }
}