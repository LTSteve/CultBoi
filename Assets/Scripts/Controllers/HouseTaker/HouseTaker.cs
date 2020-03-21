using UnityEngine;
using System;

public class HouseTaker : MonoBehaviour, IHouseTaker
{
    public float DamageToDoors = 5f;
    public float DoorDamage { get; private set; }

    void Start()
    {
        DoorDamage = DamageToDoors;
    }
}