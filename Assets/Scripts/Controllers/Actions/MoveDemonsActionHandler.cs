using UnityEngine;

public class MoveDemonsActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 2;

    public float CommandRadius = 2f;

    private Controller demonPrefab;
    private Controller me;
    private IFormationHandler formation;

    void Start()
    {
        formation = GetComponent<IFormationHandler>();

        me = GetComponent<Controller>();
    }

    public void HandleAction(IIntentManager intent)
    {
        var acting = (ActionNumber == 1 && intent.action1) ||
            (ActionNumber == 2 && intent.action2) ||
            (ActionNumber == 3 && intent.action3);

        var moveToPoint = transform.position;

        if (acting)
        {
            var mouseRay = Camera.main.ScreenPointToRay(intent.mouseLocation);
            var intersectionPlane = new Plane(Vector3.up, transform.position);
            intersectionPlane.Raycast(mouseRay, out var enter);
            moveToPoint = mouseRay.GetPoint(enter);
        }

        if(Vector3.Distance(moveToPoint, transform.position) > CommandRadius)
        {
            moveToPoint = transform.position + (moveToPoint - transform.position).normalized * CommandRadius;
        }

        MessageHandler.SendMessage<Command>(new Command
        {
            Type = moveToPoint == transform.position ? CommandType.Formation : CommandType.Move,
            Location = moveToPoint,
            From = me,
            Formation = formation
        });
    }
}