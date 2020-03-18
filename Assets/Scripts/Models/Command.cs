using UnityEngine;

public enum CommandType
{
    Move,
    Action1,
    Action2,
    Action3,
    Stand,
    Formation
}

public class Command
{
    public Vector3 Location;
    public CommandType Type;
    public Controller From;

    public IFormationHandler Formation { get; internal set; }
}