using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public int NumberOfRays = 10;

    public float SplatterDistance = 5f;

    public string[] Mask = new string[] { "Objects", "Floor" };

    public Transform BloodPrefab;

    public Sprite[] FloorBlood;
    public Sprite[] WallBlood;
    public Sprite[] LightBlood;

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
                    var rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                    Sprite sprite = null;

                    if(hitInfo.distance > (SplatterDistance * 0.7f))
                    {
                        sprite = LightBlood[Random.Range(0, LightBlood.Length)];
                    }
                    else if (hitInfo.normal.y > hitInfo.normal.x && hitInfo.normal.y > hitInfo.normal.z)
                    {
                        sprite = FloorBlood[Random.Range(0, FloorBlood.Length)];
                    }
                    else;
                    {
                        sprite = WallBlood[Random.Range(0, WallBlood.Length)];
                    }

                    var blood = Instantiate(BloodPrefab, hitInfo.point + (transform.position - hitInfo.point) * 0.01f,
                        rotation);
                    var bloodSprite = blood.GetComponentInChildren<SpriteRenderer>();
                    bloodSprite.sprite = sprite;
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
