using UnityEngine;

public class KeyboardMouseIntentManager : MonoBehaviour, IIntentManager
{
    public Vector2? moveIntent { get; private set; } = Vector2.zero;
    public float lookIntent { get; private set; } = 0f;
    public bool action1 { get; private set; } = false;
    public bool action2 { get; private set; } = false;
    public bool action3 { get; private set; } = false;
    public Vector3? mouseLocation { get; private set; }
    public Transform targeted { get; set; }
    public Vector3? moveTarget { get; set; }
    public bool setTarget { get; private set; }
    public bool unsetTarget { get; private set; }

    public void UpdateIntent()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        moveIntent = new Vector2(x, y);

        lookIntent = (Input.GetButtonDown("rotateright") ? 1 : 0) - (Input.GetButtonDown("rotateleft") ? 1 : 0);

        action1 = Input.GetKeyDown(KeyCode.Space);

        action2 = Input.GetMouseButton(0);

        action3 = Input.GetMouseButton(1);

        mouseLocation = (action2 || action3) ? (Input.mousePosition / 2f) : (Vector3?)null;
    }
}