using UnityEngine;
using System;
using System.Collections.Generic;

public class CultistManagerHack : MonoBehaviour
{
    public static List<CultistManagerHack> cultists = new List<CultistManagerHack>();

    private void Start()
    {
        cultists.Add(this);

        var death = GetComponent<IHealthHandler>();

        if (death == null) return;

        death.Died += OnDeath;
    }

    private void OnDeath(Transform obj)
    {
        if (cultists.Contains(this))
        {
            cultists.Remove(this);
        }
    }
}