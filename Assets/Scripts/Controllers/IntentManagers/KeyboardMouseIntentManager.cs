using UnityEngine;

public class KeyboardMouseIntentManager : MonoBehaviour, IIntentManager
{

    public static bool mouseMode = true;

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
    public bool Teleport { get; private set; } = true;

    public void UpdateIntent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseMode = true;
        }

        if (Input.GetButtonDown("knock on door") || Input.GetButtonDown("attack") || Mathf.Abs(Input.GetAxisRaw("verticalright")) > 0.8 || Mathf.Abs(Input.GetAxisRaw("horizontalright")) > 0.8)
        {
            mouseMode = false;
        }

        var thumbSticks = new Vector2(Input.GetAxisRaw("verticalright"), Input.GetAxisRaw("horizontalright"));
        if(thumbSticks.magnitude < 0.1f)
        {
            thumbSticks = Vector2.zero;
        }

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        moveIntent = new Vector2(x, y);

        moveIntent = moveIntent.Value.magnitude < 0.2f ? Vector2.zero : moveIntent;

        lookIntent = (Input.GetButtonDown("rotateright") ? 1 : 0) - (Input.GetButtonDown("rotateleft") ? 1 : 0);

        action1 = Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("open buy menu");

        action2 = (mouseMode && Input.GetMouseButton(0)) || (!mouseMode && (thumbSticks.x != 0 || thumbSticks.y != 0) && Input.GetAxis("attack") == 0);

        action3 = Input.GetMouseButton(1) || (!mouseMode && Input.GetAxis("attack") != 0);

        mouseLocation = mouseMode ? ((action2 || action3) ? (Input.mousePosition / 2f) : (Vector3?)null) :
            (_getMouseLocFromJoystick(thumbSticks.x, thumbSticks.y));
    }

    private Vector2 _getMouseLocFromJoystick(float x, float y)
    {
        return new Vector2(Camera.main.pixelWidth * ((1 + x) / 2f), Camera.main.pixelHeight * ((1 + y) / 2f));
    }
}