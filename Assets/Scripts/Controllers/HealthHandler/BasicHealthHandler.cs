using System;
using UnityEngine;

public class BasicHealthHandler : MonoBehaviour, IHealthHandler
{
    public float Health = 100f;
    public float HealthStart = 100f;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public Action<Transform> Died { get; set; }
    public Action<Transform, float> Damaged { get; set; }

    void Start()
    {
        MaxHealth = Health;

        CurrentHealth = HealthStart;
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;

        Damaged?.Invoke(transform, amount);

        if (CurrentHealth < 0)
        {
            Died?.Invoke(transform);
        }
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
    }
}