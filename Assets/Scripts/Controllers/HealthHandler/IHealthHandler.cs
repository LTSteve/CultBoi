using System;
using UnityEngine;

public interface IHealthHandler
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    Action<Transform> Died { get; set; }
    Action<Transform, float> Damaged { get; set; }

    void Damage(float amount);
}