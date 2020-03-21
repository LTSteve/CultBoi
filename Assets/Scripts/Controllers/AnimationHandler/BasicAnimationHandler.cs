using System;
using UnityEngine;

public class BasicAnimationHandler : MonoBehaviour, IAnimationHandler
{
    private IMover mover;
    private AttackActionHandler attack;
    private IHealthHandler damage;

    private Animator myAnimator;

    private Vector3 animatorScale;
    private Vector3 animatorRotation;

    private Material myMaterial;

    void Start()
    {
        mover = GetComponent<IMover>();
        attack = GetComponent<AttackActionHandler>();
        damage = GetComponent<IHealthHandler>();

        myAnimator = GetComponentInChildren<Animator>();
        if (myAnimator == null) return;

        myMaterial = GetComponentInChildren<Renderer>().material;

        animatorScale = myAnimator.transform.localScale;
        animatorRotation = myAnimator.transform.localRotation.eulerAngles;

        if (mover != null)
        {
            mover.Moving += OnMoving;
        }

        if(attack != null)
        {
            attack.OnAttack += OnAttack;
        }

        if(damage != null)
        {
            damage.Damaged += OnDamage;
        }
    }

    private void OnMoving(bool moving, Vector3 direction)
    {
        myAnimator.SetBool("Walking", moving);
        _applyRelativeRightNess(direction);
    }

    private void OnDamage(Transform other, float amount)
    {
        myAnimator.SetTrigger("Hurt");
    }

    private void OnAttack(Vector3 direction)
    {
        myAnimator.SetTrigger("Attack");
        _applyRelativeRightNess(direction);
    }

    private void _applyRelativeRightNess(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        //figure out relative angle
        var rightDir = Camera.main.transform.right;
        var facingDir = direction.normalized;

        var rightNess = Vector3.Dot(rightDir, facingDir);
        rightNess = rightNess >= 0 ? 1 : -1;

        myAnimator.transform.localScale = new Vector3(animatorScale.x * rightNess, animatorScale.y, animatorScale.z);

        var myProperties = myMaterial.GetTexturePropertyNames();
        if(myMaterial.HasProperty("Vector1_5F0D68EE"))
            myMaterial.SetFloat("Vector1_5F0D68EE", rightNess);
    }
}