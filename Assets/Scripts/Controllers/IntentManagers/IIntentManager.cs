using UnityEngine;

public interface IIntentManager
{
    Vector2 moveIntent { get; }
    float lookIntent { get; }
    bool action1 { get; }
    bool action2 { get; }
    bool action3 { get; }
    Vector3 mouseLocation { get; }
}