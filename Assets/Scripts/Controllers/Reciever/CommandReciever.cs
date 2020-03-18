using System;
using UnityEngine;
public class CommandReciever : MonoBehaviour, IReciever<Command>
{
    public Command activeValue { get; private set; }
    public Command defaultValue { get; private set; } = new Command { Type = CommandType.Stand };

    void Start()
    {
        MessageHandler.RegisterReciever<Command>(OnRecieveCommand);
    }

    private void OnRecieveCommand(Command command)
    {
        activeValue = command;
    }
}
