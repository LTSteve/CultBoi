using UnityEngine;

public interface IIntentManager
{
    Vector2? moveIntent { get; }
    Vector3? moveTarget { get; set; }
    float lookIntent { get; }
    bool action1 { get; }
    bool action2 { get; }
    bool action3 { get; }
    Vector3? mouseLocation { get; }
    bool setTarget { get; }
    bool unsetTarget { get; }
    bool Teleport { get; }

    void UpdateIntent();
}