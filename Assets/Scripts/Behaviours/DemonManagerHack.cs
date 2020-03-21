using UnityEngine;
using System;
using System.Collections.Generic;

public class DemonManagerHack : MonoBehaviour
{
    public int demonType = 0;
    public int demonUpgrade = 0;

    public static Dictionary<int, List<DemonManagerHack>> demons = new Dictionary<int, List<DemonManagerHack>>();

    private void Start()
    {
        if (demons.ContainsKey(demonType))
        {
            demons[demonType].Add(this);
        }
        else
        {
            demons[demonType] = new List<DemonManagerHack> { this };
        }
    }
}