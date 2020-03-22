using UnityEngine;

public class BasicDeathHandler : MonoBehaviour, IDeathHandler
{
    public Transform DeathEffectPrefab;

    public Vector3 SpatterPoint = new Vector3(0, 2, 0);

    public bool DeathAnimation = true;

    private IAnimationHandler anims;

    void Start()
    {
        anims = GetComponent<IAnimationHandler>();

        var health = GetComponent<IHealthHandler>();

        if (health == null) return;

        health.Died += Died;
    }

    public void Died(Transform transform)
    {
        if (DeathEffectPrefab != null) Instantiate(DeathEffectPrefab, transform.position + SpatterPoint, Quaternion.identity);

        var intentManager = GetComponent<DemonIntentManager>();
        if(intentManager != null && intentManager.formation != null)
        {
            intentManager.formation.Remove(transform);
        }

        if (!DeathAnimation)
            Destroy(this.gameObject);
    }
}