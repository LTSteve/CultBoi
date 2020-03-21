using System;
using UnityEngine;

public interface ITriggerHandler
{
    Action<Collider> IsTriggered { get; set; }
    Action<Collider> IsUnTriggered { get; set; }
}