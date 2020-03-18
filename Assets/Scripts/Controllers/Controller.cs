using System.Collections.Generic;
using UnityEngine;
public class Controller : MonoBehaviour
{
    private IIntentManager intentManager;
    private IMover mover;
    private IRotator rotator;

    private IEnumerable<IActionHandler> actions;

    void Start()
    {
        intentManager = GetComponent<IIntentManager>();
        mover = GetComponent<IMover>();
        rotator = GetComponent<IRotator>();

        actions = GetComponents<IActionHandler>();
    }

    void Update()
    {
        if(mover != null)
            mover.Move(intentManager.moveIntent);

        if(rotator != null)
            rotator.Rotate(intentManager.lookIntent);

        if(actions != null)
            foreach (var action in actions)
            {
                action.HandleAction(intentManager);
            }
    }
}
