using UnityEngine;

public class MoveDemonsActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 2;

    private Controller demonPrefab;

    private Controller me;

    void Start()
    {
        me = GetComponent<Controller>();
    }

    public void HandleAction(IIntentManager intent)
    {
        if (ActionNumber < 1 || ActionNumber > 3) return;
        if (ActionNumber == 1 && !intent.action1) return;
        if (ActionNumber == 2 && !intent.action2) return;
        if (ActionNumber == 3 && !intent.action3) return;

        var mouseRay = Camera.main.ScreenPointToRay(intent.mouseLocation);

        var intersectionPlane = new Plane(Vector3.up, transform.position);

        intersectionPlane.Raycast(mouseRay, out var enter);

        MessageHandler.SendMessage<Command>(new Command
        {
            Type = CommandType.Move,
            Location = mouseRay.GetPoint(enter),
            From = me
        });
    }
}