using System;
using UnityEngine;

public abstract class CommandDemonsActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 2;
    public float CommandRadius = 2f;

    protected bool wasActive = false;

    public virtual void HandleAction(IIntentManager intent)
    {
        var acting = (ActionNumber == 1 && intent.action1) ||
            (ActionNumber == 2 && intent.action2) ||
            (ActionNumber == 3 && intent.action3);

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

    //optional override to change how the location is gathered
    protected virtual Vector3 GetCommandLocation(IIntentManager intent)
    {
        var mouseRay = Camera.main.ScreenPointToRay(intent.mouseLocation.HasValue ? intent.mouseLocation.Value : Vector3.zero);
        var intersectionPlane = new Plane(Vector3.up, transform.position);
        intersectionPlane.Raycast(mouseRay, out var enter);
        return mouseRay.GetPoint(enter);
    }

    protected abstract Command BuildCommand(Vector3 commandLocation);
}