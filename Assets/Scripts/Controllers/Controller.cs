using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Player,
    Demon,
    Pedestrian,
    Enemy
}

public class Controller : MonoBehaviour
{
    public Faction Faction;

    protected IIntentManager intentManager;
    protected IMover mover;
    protected IRotator rotator;
    protected IHealthHandler health;

    protected IEnumerable<IActionHandler> actions;

    protected virtual void Start()
    {
        intentManager = GetComponent<IIntentManager>();
        mover = GetComponent<IMover>();
        rotator = GetComponent<IRotator>();
        health = GetComponent<IHealthHandler>();

        actions = GetComponents<IActionHandler>();
    }

    protected virtual void Update()
    {
        if(intentManager != null)
            intentManager.UpdateIntent();

        if (mover != null)
            mover.Move(intentManager);

        if (rotator != null)
            rotator.Rotate(intentManager.lookIntent);

        if(actions != null)
            foreach (var action in actions)
            {
                action.HandleAction(intentManager);
            }
    }
}
