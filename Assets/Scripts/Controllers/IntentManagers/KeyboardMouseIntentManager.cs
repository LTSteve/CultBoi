using UnityEngine;

public class KeyboardMouseIntentManager : MonoBehaviour, IIntentManager
{
    public Vector2 moveIntent { get; private set; } = Vector2.zero;

    public float lookIntent { get; private set; } = 0f;

    public bool action1 { get; private set; } = false;
    public bool action2 { get; private set; } = false;
    public bool action3 { get; private set; } = false;
    public Vector3 mouseLocation { get; private set; }

    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        moveIntent = new Vector2(x, y);

        lookIntent = (Input.GetButtonDown("rotateright") ? 1 : 0) - (Input.GetButtonDown("rotateleft") ? 1 : 0);

        action1 = Input.GetKeyDown(KeyCode.Space);

        action2 = Input.GetMouseButton(0);

        mouseLocation = Input.mousePosition;
    }
}