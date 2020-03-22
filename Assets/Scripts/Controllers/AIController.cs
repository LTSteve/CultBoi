using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    protected override void Update()
    {
        if (PauseMenu.IsOpen) return;

        intentManager.UpdateIntent();

        if (mover != null)
        {
            mover.Move(intentManager);
        }

        if(actions != null)
            foreach (var action in actions)
            {
                action.HandleAction(intentManager);
            }
    }
}
