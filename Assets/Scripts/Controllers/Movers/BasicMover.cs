using System;
using UnityEngine;

public class BasicMover : MonoBehaviour, IMover
{
    public float MoveSpeed = 10f;

    public float ObjectCollisionDistance = 1f;

    public float StuckSaftyDistance = 0.1f;

    public float AdvanceToRange = 4f;

    public string[] ObjectsMask = { "Objects" };
    public Action<bool, Vector3> Moving { get; set; }

    protected Transform moveTarget;

    protected ITargetingHandler targeting;

    protected virtual void Start()
    {
        targeting = GetComponent<ITargetingHandler>();
    }

    public virtual void Move(IIntentManager intent)
    {
        var point = intent.moveTarget;
        var moveIntent = intent.moveIntent;

        if(moveTarget != null || intent.setTarget)
        {
            _moveToTarget(intent);
            return;
        }
        else if (point.HasValue)
        {
            _moveToPoint(point.Value);
            return;
        }
        else if(moveIntent.HasValue)
        {
            _moveToIntent(moveIntent.Value);
            return;
        }

        Moving?.Invoke(false, Vector3.zero);
    }

    protected virtual void _moveToIntent(Vector2 moveIntent)
    {
        _move(transform.rotation * new Vector3(moveIntent.x, 0, moveIntent.y));
    }

    protected virtual void _moveToPoint(Vector3 moveTo)
    {
        _move((moveTo - transform.position).normalized);
    }

    protected virtual void _moveToTarget(IIntentManager intent)
    {
        if (intent.unsetTarget)
        {
            moveTarget = null;
        }

        if (targeting != null && moveTarget == null && !intent.unsetTarget)
        {
            moveTarget = targeting.AcquireTarget();
            if(moveTarget != null)
            {
                Debug.Log("movetome");
            }
        }

        if (moveTarget == null)
        {
            if(intent.moveTarget.HasValue)
                _moveToPoint(intent.moveTarget.Value);
            Moving?.Invoke(false, Vector3.zero);
            return;
        }

        if (Vector3.Distance(transform.position, moveTarget.position) <= AdvanceToRange)
        {
            Moving?.Invoke(false, Vector3.zero);
            return;
        }

        _moveToPoint(moveTarget.position);
    }

    protected virtual void _move(Vector3 moveDir)
    {
        var movement = _physicalize(moveDir * MoveSpeed * Time.deltaTime);

        Moving?.Invoke(movement != Vector3.zero, movement);

        transform.position += movement;
    }

    protected Vector3 _physicalize(Vector3 movement)
    {
        movement = new Vector3(movement.x, 0, movement.z);

        var checkDir = movement + movement.normalized * ObjectCollisionDistance;

        var checkRay = new Ray(transform.position, checkDir.normalized);

        if (Physics.Raycast(checkRay, out var hit, checkDir.magnitude, LayerMask.GetMask(ObjectsMask)) && hit.distance > StuckSaftyDistance)
        {
            //move partly there
            var partialMovementMag = hit.distance - ObjectCollisionDistance;
            transform.position += partialMovementMag * movement.normalized;

            //turn the rest of the vector
            var remainingMag = (movement.magnitude - partialMovementMag);
            movement = movement.normalized * remainingMag;

            var penetration = -Vector3.Dot(hit.normal, movement);
            movement += hit.normal * penetration;

            return movement.normalized * remainingMag;
        }
        else
        {
            return movement;
        }
    }
}