using System.Collections.Generic;
using UnityEngine;

public class ForceMover : BasicMover
{
    public float Acceleration = 1f;

    public float SpeedLimitScale = 3f;

    public float StopSeconds = 1f;

    public float PushedForce = 10f;

    public string[] Mask = new string[]
    {
        "Movers"
    };

    private SphereCollider myCollider;

    private Vector3 velocity;

    private Collider[] activeCollisions;

    private Vector3 forceDir;
    protected override void Start()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    public override void Move(IIntentManager intent)
    {
        forceDir = Vector3.zero;

        var moveForce = MoveSpeed * Acceleration;
        
        base.Move(intent);

        if (activeCollisions != null)
            foreach (var col in activeCollisions)
            {
                var distance = Vector3.Distance(transform.position, col.transform.position) - myCollider.radius;

                var pushLevel = Mathf.Clamp((1f - (distance / myCollider.radius)), 0f, 1f);

                forceDir += pushLevel * PushedForce * (transform.position - col.transform.position).normalized;
            }

        //friction
        velocity -= velocity * Time.deltaTime / StopSeconds;

        velocity += forceDir * moveForce * Time.deltaTime;

        if (velocity.magnitude > SpeedLimitScale * MoveSpeed)
        {
            velocity = velocity.normalized * SpeedLimitScale;
        }
        transform.position += _physicalize(velocity * Time.deltaTime);
    }

    protected override void _moveToIntent(Vector2 moveIntent)
    {
        forceDir = transform.rotation * new Vector3(moveIntent.x, 0, moveIntent.y);
    }

    protected override void _moveToPoint(Vector3 moveTo)
    {
        forceDir = (moveTo - transform.position).normalized;
    }

    void FixedUpdate()
    {
        activeCollisions = Physics.OverlapSphere(transform.position, myCollider.radius, LayerMask.GetMask(Mask));
    }
}