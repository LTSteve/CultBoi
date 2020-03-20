using UnityEngine;
using System;

public abstract class AttackActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    public float AttackRange = 4f;

    public float AttackRate = 1f;
    public float AttackDamage = 5f;

    public Action<Vector3> OnAttack { get; set; }

    protected Transform target;

    protected bool acting;

    protected ITargetingHandler targeting;

    protected float attackCooldown = 0;

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
        else if (target == null)
        {
            target = targeting.AcquireTarget();
        }

        if (target == null) return;

        var targetDistance = Vector3.Distance(target.position, transform.position);

        handleAttack(Vector3.Distance(target.position, transform.position), target);
    }

    private void handleAttack(float distance, Transform target)
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown > 0)
        {
            return;
        }

        if (distance > AttackRange)
        {
            return;
        }

        var enemy = target.GetComponent<IHealthHandler>();

        if (enemy != null) { enemy.Damage(AttackDamage); OnAttack?.Invoke(target.position - transform.position); }

        attackCooldown = AttackRate;
    }
}