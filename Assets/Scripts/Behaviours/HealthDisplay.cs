using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Image Bar;

    private Color baseColor;
    private bool full = true;
    private float disappearTimer = 1f;

    private IHealthHandler health;

    void Start()
    {
        Bar = GetComponentInChildren<Image>();
        baseColor = Bar.color;
        Bar.color = Color.clear;
        health = GetComponentInParent<IHealthHandler>();

        if(health != null)
        {
            health.Damaged += SetPercent;
        }
    }

    private void Update()
    {
        if (!full)
        {
            Bar.color = baseColor;
            return;
        }

        disappearTimer -= Time.deltaTime;

        Bar.color = Color.Lerp(baseColor, Color.clear, 1 - disappearTimer);
    }

    private void SetPercent(Transform transform, float damage)
    {
        var percent = health.CurrentHealth / health.MaxHealth;

        percent = Mathf.Clamp(percent, 0, 1);

        Bar.transform.localScale = new Vector3(percent, 1, 1);

        full = percent == 1;

        if (full) disappearTimer = 1;
    }
}