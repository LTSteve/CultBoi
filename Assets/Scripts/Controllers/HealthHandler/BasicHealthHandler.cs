﻿using System;
using UnityEngine;

public class BasicHealthHandler : MonoBehaviour, IHealthHandler
{
    public float Health = 100f;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public Action<Transform> Died { get; set; }
    public Action<Transform, float> Damaged { get; set; }

    void Start()
    {
        MaxHealth = CurrentHealth = Health;
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;

        Damaged(transform, amount);

        if(CurrentHealth < 0)
        {
            Died(transform);
        }
    }
}