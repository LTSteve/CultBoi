using System;
using UnityEngine;

public class BasicMover : MonoBehaviour, IMover
{
    public float MoveSpeed = 10f;

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
        }
        else if (point.HasValue)
        {
            _moveToPoint(point.Value);
        }
        else if(moveIntent.HasValue)
        {
            _moveToIntent(moveIntent.Value);
        }
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
        
        if (moveTarget == null && intent.moveTarget.HasValue)
        {
            _moveToPoint(intent.moveTarget.Value);
            return;
        }

        if (moveTarget != null)
            _moveToPoint(moveTarget.position);
        else if (intent.moveTarget.HasValue)
            _moveToPoint(intent.moveTarget.Value);

    }

    protected virtual void _move(Vector3 moveDir)
    {
        transform.position += moveDir * MoveSpeed * Time.deltaTime;
    }
}