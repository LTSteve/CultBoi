using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldGenerator : MonoBehaviour
{

    public List<Transform> WorldTiles;

    public List<Transform> Branches;  

    public int LevelLength = 10;

    public float BranchLikelihood = 0.5f;

    public float TileWidth = 10f;

    private bool generated = false;

    private List<Transform> map = new List<Transform>();

    private List<Transform> branches = new List<Transform>();

    void Start()
    {
        
    }

    void Update()
    {
        if (!generated)
        {
            generated = true;
            StartCoroutine(generateWorld());
        }
    }

    private IEnumerator generateWorld()
    {
        map.Add(Instantiate(WorldTiles[0], transform.position, Quaternion.identity, transform));

        yield return null;

        bool skip = false; // skip branch after generating one last time

        for(var i = 1; i < LevelLength - 1; i++) //skip the start & end
        {
            var offset = transform.position + Vector3.right * TileWidth * i;

            var branch = Random.value >= BranchLikelihood;

            if (!skip && branch)
            {
                var upDownBoth = Random.Range(0, 3);

                switch (upDownBoth)
                {
                    case 0: //up
                        map.Add(Instantiate(WorldTiles[4], offset, Quaternion.identity, transform));
                        GenerateBranch(offset + Vector3.forward * TileWidth, Quaternion.Euler(0, -90, 0));
                        break;
                    case 1: //down
                        map.Add(Instantiate(WorldTiles[4], offset, Quaternion.Euler(0, 180, 0), transform));
                        GenerateBranch(offset + Vector3.back * TileWidth, Quaternion.Euler(0, 90, 0));
                        break;
                    case 2:
                        map.Add(Instantiate(WorldTiles[1], offset, Quaternion.identity, transform));
                        GenerateBranch(offset + Vector3.forward * TileWidth, Quaternion.Euler(0, -90, 0));
                        GenerateBranch(offset + Vector3.back * TileWidth, Quaternion.Euler(0, 90, 0));
                        break;
                }

                skip = true;
            }
            else
            {
                map.Add(Instantiate(WorldTiles[2], offset, Quaternion.identity, transform));

                skip = false;
            }

            yield return null;
        }

        map.Add(Instantiate(WorldTiles[0], transform.position + Vector3.right * TileWidth * 9, Quaternion.Euler(0, 180, 0), transform));
    }

    private void GenerateBranch(Vector3 offset, Quaternion rotation)
    {
        var branchNum = Random.Range(0, Branches.Count);

        var newBranch = Instantiate(Branches[branchNum], offset, rotation, transform);
        branches.Add(newBranch);

        foreach(Transform tile in newBranch)
        {
            map.Add(tile);
        }
    }
}
