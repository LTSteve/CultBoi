using System;
using UnityEngine;

public abstract class CommandDemonsActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 2;
    public float CommandRadius = 2f;

    protected bool wasActive = false;

    public Transform Target;

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

            if (Target != null)
            {
                Target.gameObject.SetActive(true);

                Target.position = commandLocation;
            }

            MessageHandler.SendMessage(command);
            wasActive = true;
        }
        else if (wasActive)
        {
            wasActive = false;
            MessageHandler.SendMessage<Command>(null);

            if (Target != null)
            {
                Target.gameObject.SetActive(false);

                Target.position = commandLocation;
            }
        }
    }

    private Ray? rayBoi = null;

    //optional override to change how the location is gathered
    protected virtual Vector3 GetCommandLocation(IIntentManager intent)
    {
        var mouseLoc = intent.mouseLocation.HasValue ? intent.mouseLocation.Value : Vector3.zero;

        rayBoi = Camera.main.ScreenPointToRay(mouseLoc);
        var intersectionPlane = new Plane(Vector3.up, transform.position);
        intersectionPlane.Raycast(rayBoi.Value, out var enter);
        return rayBoi.Value.GetPoint(enter);
    }

    private void OnDrawGizmos()
    {
        if(rayBoi.HasValue)
        {
            Gizmos.DrawRay(rayBoi.Value);
        }
    }

    protected abstract Command BuildCommand(Vector3 commandLocation);
}