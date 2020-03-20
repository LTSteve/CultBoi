using UnityEngine;

public class BasicDeathHandler : MonoBehaviour, IDeathHandler
{
    void Start()
    {
        var health = GetComponent<IHealthHandler>();

        if (health == null) return;

        health.Died += Died;
    }

    public void Died(Transform transform)
    {
        Destroy(this.gameObject);
    }
}