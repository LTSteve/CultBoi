using System.Collections.Generic;
using UnityEngine;

public class WorldTileSubtype : MonoBehaviour
{
    public float HouseSpawnRate = 0.5f;

    private void Start()
    {
        var housePrefab = Resources.Load<Transform>("Prefab/Objects/House");

        var houseSpawnPoints = transform.Find("HouseSpawnPoints");

        foreach (Transform houseSpawnPoint in houseSpawnPoints)
        {
            if (Random.value > HouseSpawnRate)
            {
                continue;
            }

            Instantiate(housePrefab, houseSpawnPoint);
        }
    }
}
