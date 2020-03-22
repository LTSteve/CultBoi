using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;

    public Transform[] Hearts;

    public BasicHealthHandler PlayerHealth;

    private void Awake()
    {
        Instance = this;

        PlayerHealth.Damaged += OnDamaged;
    }

    private void OnDamaged(Transform by, float amount)
    {
        var percent = Mathf.Clamp(PlayerHealth.CurrentHealth / PlayerHealth.MaxHealth, 0, 0.99f);

        var heartNo = percent == 0 ? 0 : (int)(percent * 3) + 1;

        for(var i = 0; i < Hearts.Length; i++) 
        {
            Hearts[i].gameObject.SetActive(heartNo > i);
        }
    }

    public void Reset()
    {

        for (var i = 0; i < Hearts.Length; i++)
        {
            Hearts[i].gameObject.SetActive(true);
        }
    }
}
