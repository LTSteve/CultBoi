using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour, IDeathHandler
{
    public Transform DeathEffectPrefab;

    public Vector3 SpatterPoint = new Vector3(0, 2, 0);

    void Start()
    {

        var health = GetComponent<IHealthHandler>();

        if (health == null) return;

        health.Died += Died;
    }

    public void Died(Transform transform)
    {
        if (DeathEffectPrefab != null) Instantiate(DeathEffectPrefab, transform.position + SpatterPoint, Quaternion.identity);

        StartCoroutine(_slowDeath());
        DeathScreen.Instance.Open(transform);
    }

    IEnumerator _slowDeath()
    {
        yield return new WaitForSeconds(1f);
    }
}