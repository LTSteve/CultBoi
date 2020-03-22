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

    private AudioSource oofAudio;
    public AudioClip OofClip;

    void Start()
    {
        MaxHealth = Health;

        CurrentHealth = HealthStart;

        if(OofClip != null)
        {
            oofAudio = transform.Find("Audio")?.GetComponent<AudioSource>();
            if (oofAudio == null)
            {
                oofAudio = GetComponentInChildren<AudioSource>();
            }
        }
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;

        Damaged?.Invoke(transform, amount);

        if (CurrentHealth < 0)
        {
            Died?.Invoke(transform);
        }
        else
        {
            if(oofAudio != null)
            {
                oofAudio.PlayOneShot(OofClip);
            }
        }
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
    }
}