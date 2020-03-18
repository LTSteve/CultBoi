using UnityEngine;

public enum CommandType
{
    Move,
    Action1,
    Action2,
    Action3,
    Stand
}

public class Command
{
    public Vector3 Location;
    public CommandType Type;
    public Controller From;
}