using UnityEngine;

public class BasicAttackActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    public float AttackRange = 4f;

    public float AttackRate = 1f;
    public float AttackDamage = 5f;

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

        var targetDistance = Vector3.Distance(target.position, transform.position);

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

        if (enemy != null) { enemy.Damage(AttackDamage); TempDrawLine(transform.position, target.position); }

        attackCooldown = AttackRate;
    }

    private void TempDrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.white;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, 0.2f);
    }
}