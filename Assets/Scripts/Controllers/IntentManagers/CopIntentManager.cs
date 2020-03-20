using UnityEngine;

public class CopIntentManager : MonoBehaviour, IIntentManager
{
    public Vector2? moveIntent { get; private set; } = Vector2.zero;
    public Vector3? moveTarget { get; set; } = Vector3.zero;
    public bool unsetTarget { get; private set; }
    public bool setTarget { get; private set; }
    public float lookIntent { get; private set; } = 0f;
    public bool action1 { get; private set; } = false;
    public bool action2 { get; private set; } = false;
    public bool action3 { get; private set; } = false;
    public Vector3? mouseLocation { get; private set; } = null;

    public void UpdateIntent()
    {

    }
}