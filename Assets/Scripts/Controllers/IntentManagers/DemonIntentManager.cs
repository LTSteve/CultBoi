using UnityEngine;

public class DemonIntentManager : MonoBehaviour, IIntentManager
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

    public float DoubletimeModifier = 2f;


    private IReciever<Command> commandReciever;

    [HideInInspector]
    public IFormationHandler formation;
    [HideInInspector]
    public Transform parent;

    private CommandType LastCommand;

    void Start()
    {
        commandReciever = GetComponent<IReciever<Command>>();
    }

    public void UpdateIntent()
    {
        moveIntent = null;
        moveTarget = null;
        setTarget = false;
        unsetTarget = false;

        action1 = action2 = action3 = false;

        var activeCommand = commandReciever.activeValue;

        if(activeCommand != null && activeCommand.Type == CommandType.Move)
        {
            moveTarget = activeCommand.Location;
            unsetTarget = true;
        }

        if(activeCommand == null && LastCommand == CommandType.Move)
        {
            var formationPosition = formation.GetMyPosition(this);

            moveIntent = ParseMoveIntent(formationPosition, parent.position);
        }

        action1 = (activeCommand != null && activeCommand.Type == CommandType.Action1) || LastCommand == CommandType.Action1;
        action2 = (activeCommand != null && activeCommand.Type == CommandType.Action2) || LastCommand == CommandType.Action2;
        action3 = (activeCommand != null && activeCommand.Type == CommandType.Action3) || LastCommand == CommandType.Action3;

        if(action1 || action2 || action3)
        {
            if(activeCommand != null)
            {
                moveTarget = activeCommand.Location;
            }
            else
            {
                //Attack towards formation if no active command
                var formationPosition = formation.GetMyPosition(this);

                moveIntent = ParseMoveIntent(formationPosition, parent.position);
            }
            setTarget = true;
        }

        if(activeCommand != null)
            LastCommand = activeCommand.Type;
    }

    private Vector3 ParseMoveIntent(Vector3 commandLocation, Vector3 commandFrom)
    {
        var movingVec = commandLocation - transform.position;
        var moveDir = movingVec.magnitude > 1f ? (commandLocation - transform.position).normalized : movingVec;

        if (Vector3.Distance(commandLocation, commandFrom) < Vector3.Distance(commandLocation, transform.position))
        {
            moveDir *= DoubletimeModifier;
        }

        return new Vector2(moveDir.x, moveDir.z);
    }
}