using System.Collections;
using UnityEngine;

public class BasicDeathHandler : MonoBehaviour, IDeathHandler
{
    public Transform DeathEffectPrefab;

    public Vector3 SpatterPoint = new Vector3(0, 2, 0);

    public bool DeathAnimation = true;

    private IAnimationHandler anims;

    private AudioSource oofAudio;
    public AudioClip DieClip;

    void Start()
    {
        anims = GetComponent<IAnimationHandler>();

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

        var intentManager = GetComponent<DemonIntentManager>();
        if(intentManager != null && intentManager.formation != null)
        {
            intentManager.formation.Remove(transform);
        }

        if (DeathAnimation)
        {
            if(DieClip != null)
                oofAudio?.PlayOneShot(DieClip);
        }
        else if (!DeathAnimation)
        {
            if(oofAudio != null && DieClip!= null)
                StartCoroutine(CryThenDie());
            else
                Destroy(this.gameObject);
        }
    }

    public IEnumerator CryThenDie()
    {
        oofAudio.PlayOneShot(DieClip);

        foreach(var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}