using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldTileType {
    Blocked,
    DeadEnd,
    Intersection,
    Offroad,
    Straight,
    T,
    Turn
}

public class WorldTile : MonoBehaviour
{
    private static Dictionary<WorldTileType, WorldTileSubtype[]> SubTypesCache = new Dictionary<WorldTileType, WorldTileSubtype[]>();

    public WorldTileType MyType;

    public bool ShowDebugFloor = false;

    void Start()
    {
        if (!ShowDebugFloor)
        {
            var debugFloor = transform.Find("Floor");

            if(debugFloor != null)
                Destroy(debugFloor.gameObject);
        }
    }

    public WorldTileSubtype SpawnSubtype()
    {
        if (!SubTypesCache.ContainsKey(MyType))
        {
            SubTypesCache[MyType] = Resources.LoadAll<WorldTileSubtype>("Prefab/WorldTiles/" + MyType.ToString());
        }

        var subTypes = SubTypesCache[MyType];

        var toSpawn = subTypes[Random.Range(0, subTypes.Length - 1)];

        return Instantiate(toSpawn, transform);
    }
}
