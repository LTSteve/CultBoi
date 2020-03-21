using UnityEngine;
using System;

public class HouseTaker : MonoBehaviour, IHouseTaker
{
    public float DamageToDoors = 5f;
    public float DoorDamage { get; private set; }

    private bool _takingHouse = false;
    public bool TakingHouse { 
        get {
            return _takingHouse;
        }
        set {
            _takingHouse = value;
            if (value)
            {
                cdRemaining = Cooldown;
            }
        } }
    public float Cooldown = 0.5f;

    private float cdRemaining = 0f;

    void Start()
    {
        DoorDamage = DamageToDoors;
    }

    void Update()
    {
        if(TakingHouse && cdRemaining <= 0)
        {
            TakingHouse = false;
        }

        cdRemaining -= Time.deltaTime;
    }
}