using System;
using UnityEngine;

public class BasicMover : MonoBehaviour, IMover
{
    public float MoveSpeed = 10f;

    private Transform moveTarget;

    private ITargetingHandler targeting;

    void Start()
    {
        targeting = GetComponent<ITargetingHandler>();
    }

    public void Move(IIntentManager intent)
    {
        var point = intent.moveTarget;
        var moveIntent = intent.moveIntent;

        if(moveTarget != null)
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

    private void _moveToIntent(Vector2 moveIntent)
    {
        _move(transform.rotation * new Vector3(moveIntent.x, 0, moveIntent.y));
    }

    private void _moveToPoint(Vector3 moveTo)
    {
        _move((moveTo - transform.position).normalized);
    }

    private void _moveToTarget(IIntentManager intent)
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

        _moveToPoint(moveTarget.position);
    }

    private void _move(Vector3 moveDir)
    {
        transform.position += moveDir * MoveSpeed * Time.deltaTime;
    }
}