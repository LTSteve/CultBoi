using UnityEngine;

public class MoveDemonsActionHandler : CommandDemonsActionHandler
{

    public override void HandleAction(IIntentManager intent)
    {
        var acting = intent.mouseLocation.HasValue;

        var commandLocation = transform.position;

        if (acting)
        {
            commandLocation = GetCommandLocation(intent);
            var command = BuildCommand(commandLocation);

            MessageHandler.SendMessage(command);
            wasActive = true;
        }
        else if (wasActive)
        {
            wasActive = false;
            MessageHandler.SendMessage<Command>(null);
        }
    }

    protected override Command BuildCommand(Vector3 commandLocation)
    {
        return new Command
        {
            Type = CommandType.Move,
            Location = commandLocation,
            From = transform
        };
    }
}