using UnityEngine;
using System;

public interface IMover
{
    Action<bool, Vector3> Moving { get; set; }
    void Move(IIntentManager intent);
}
