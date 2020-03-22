using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour, IDeathHandler
{
    public Transform DeathEffectPrefab;

    public Vector3 SpatterPoint = new Vector3(0, 2, 0);

    private AudioSource oofAudio;
    public AudioClip DieClip;

    void Start()
    {

        var health = GetComponent<IHealthHandler>();

        if (health == null) return;

        health.Died += Died;

        if (DieClip != null)
        {
            oofAudio = transform.Find("Audio")?.GetComponent<AudioSource>();
            if (oofAudio == null)
            {
                oofAudio = GetComponentInChildren<AudioSource>();
            }
        }
    }

    public void Died(Transform transform)
    {
        if (DeathEffectPrefab != null) Instantiate(DeathEffectPrefab, transform.position + SpatterPoint, Quaternion.identity);

        oofAudio?.PlayOneShot(DieClip);
        StartCoroutine(_slowDeath());
    }

    IEnumerator _slowDeath()
    {
        yield return new WaitForSeconds(1f);
        DeathScreen.Instance.Open(transform);
    }
}