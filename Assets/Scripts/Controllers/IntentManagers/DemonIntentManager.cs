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

    private IFormationHandler formation;

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
            moveIntent = ParseMoveIntent(commandLocation, activeCommand.From.transform.position);
        }

        if(formation == null)
        {
            formation = activeCommand.Formation;
        }

        if(activeCommand.Type == CommandType.Formation)
        {
            var formationPosition = formation.GetMyPosition(this);

            moveIntent = ParseMoveIntent(formationPosition, activeCommand.From.transform.position);
        }

        action1 = activeCommand.Type == CommandType.Action1;
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