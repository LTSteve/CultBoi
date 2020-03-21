using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public int NumberOfRays = 10;

    public float SplatterDistance = 5f;

    public string[] Mask = new string[] { "Objects", "Floor" };

    public Transform BloodPrefab;

    private ParticleSystem myParticles;

    private bool firstUpdate = true;

    void Start()
    {
        myParticles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myParticles.isStopped)
        {
            Destroy(gameObject);
            return;
        }

        if (firstUpdate)
        {
            firstUpdate = false;

            for(var i = 0; i < NumberOfRays; i++)
            {
                var ray = new Ray(transform.position, Random.insideUnitSphere);

                if(Physics.Raycast(ray, out var hitInfo, SplatterDistance, LayerMask.GetMask(Mask)))
                {
                    Instantiate(BloodPrefab, hitInfo.point + (transform.position - hitInfo.point) * 0.01f, 
                        Quaternion.Euler(_nintyize(hitInfo.normal.x), _nintyize(hitInfo.normal.y), _nintyize(hitInfo.normal.z)));
                }
            }
        }
    }

    private float _nintyize(float value)
    {
        var reduced = value / 90f;

        reduced = Mathf.Round(reduced);

        return reduced * 90f;
    }
}
