using UnityEngine;

public class BasicMover : MonoBehaviour, IMover
{
    public float MoveSpeed = 10f;

    public void Move(Vector2 intent)
    {
        var moveDir = transform.rotation * new Vector3(intent.x, 0, intent.y);
        transform.position += moveDir * MoveSpeed * Time.deltaTime;
    }
}