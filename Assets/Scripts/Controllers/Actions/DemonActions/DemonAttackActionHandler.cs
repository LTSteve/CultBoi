using UnityEngine;

public class DemonAttackActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    public float AttackRange = 4f;

    private float AttackRate = 1f;
    private float AttackDamage = 5f;

    private Transform target;

    private bool acting;

    private ITargetingHandler targeting;

    private float attackCooldown = 0;

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

        handleAttack(Vector3.Distance(target.position, transform.position), target);
    }

    private void handleAttack(float distance, Transform target)
    {
        attackCooldown -= Time.deltaTime;

        if(attackCooldown > 0)
        {
            return;
        }

        if (distance > AttackRange)
        {
            return;
        }

        var enemy = target.GetComponent<IHealthHandler>();

        if (enemy != null) enemy.Damage(AttackDamage);

        attackCooldown = AttackRate;
    }
}