using System.Collections.Generic;
using UnityEngine;

public class ForceMover : MonoBehaviour, IMover
{
    public float MoveForce = 10f;

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

    void Start()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    public void Move(IIntentManager intent)
    {
        /*
        var moveForce = MoveForce * Acceleration;

        var forceDir = transform.rotation * new Vector3(intent.x, 0, intent.y);

        if(activeCollisions != null)
            foreach(var col in activeCollisions)
            {
                var distance = Vector3.Distance(transform.position, col.transform.position) - myCollider.radius;

                var pushLevel = Mathf.Clamp((1f - (distance / myCollider.radius)), 0f, 1f);

                forceDir += pushLevel * PushedForce * (transform.position - col.transform.position).normalized;
            }

        //friction
        velocity -= velocity * Time.deltaTime / StopSeconds;

        velocity += forceDir * moveForce * Time.deltaTime;

        if(velocity.magnitude > SpeedLimitScale * MoveForce)
        {
            velocity = velocity.normalized * SpeedLimitScale;
        }
        transform.position += velocity * Time.deltaTime;
        */
    }

    void FixedUpdate()
    {
        activeCollisions = Physics.OverlapSphere(transform.position, myCollider.radius, LayerMask.GetMask(Mask));
    }
}