using UnityEngine;

public class DemonIntentManager : MonoBehaviour, IIntentManager
{
    public Vector2 moveIntent { get; private set; } = Vector2.zero;

    public float lookIntent { get; private set; } = 0f;

    public bool action1 { get; private set; } = false;
    public bool action2 { get; private set; } = false;
    public bool action3 { get; private set; } = false;
    public Vector3 mouseLocation { get; private set; }

    public float DoubletimeModifier = 2f;

    private IReciever<Command> commandReciever;

    void Start()
    {
        commandReciever = GetComponent<IReciever<Command>>();
    }

    void Update()
    {
        var activeCommand = commandReciever.activeValue != null ? commandReciever.activeValue : commandReciever.defaultValue;

        var commandLocation = activeCommand.Location;

        if(activeCommand.Type != CommandType.Stand)
        {
            var moveTo = commandLocation - transform.position;
            var moveDir = moveTo.magnitude > 1f ? (commandLocation - transform.position).normalized : moveTo;

            if(Vector3.Distance(commandLocation, activeCommand.From.transform.position) < Vector3.Distance(commandLocation, transform.position))
            {
                moveDir *= DoubletimeModifier;
            }

            moveIntent = new Vector2(moveDir.x, moveDir.z);
        }

        action1 = activeCommand.Type == CommandType.Action1;
    }
}