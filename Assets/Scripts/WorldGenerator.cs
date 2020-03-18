using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldGenerator : MonoBehaviour
{

    public List<Transform> WorldTiles;

    public int LevelWidth = 10;
    public int LevelHeight = 5;
    public float BranchLikelihood = 0.5f;
    public int TurnCount = 2;
    public float OffRoadGrowthCount = 1.5f;
    public int VerticalMainRoadPadding = 1;
    public int MaxBranchDepth = 5;

    public float TileWidth = 10f;

    private List<Transform> map = new List<Transform>();

    private bool[,] roadMap;

    void Start()
    {
        roadMap = new bool[LevelWidth, LevelHeight];

        StartCoroutine(generateWorld());
    }

    void Update()
    {
    }

    private IEnumerator generateWorld()
    {
        //generate an n x m map of(false) bools
        roadMap = new bool[LevelWidth, LevelHeight];

        //grab a random start and end point from n = 0 and n = len - 1
        var vertMainRoad = UnityEngine.Random.Range(0, LevelHeight - VerticalMainRoadPadding * 2) + VerticalMainRoadPadding;
        var startPoint = new Vector2Int(0, vertMainRoad);

        //ensure the endPoint is on the opposite side of the map from the startPoint
        vertMainRoad = UnityEngine.Random.Range(0, LevelHeight / 2 - VerticalMainRoadPadding) + VerticalMainRoadPadding;
        var endPoint = new Vector2Int(LevelWidth - 1, Mathf.Clamp(
            vertMainRoad + (startPoint.y > (LevelHeight / 2) ? 0 : ((LevelHeight / 2) - VerticalMainRoadPadding)), 
            VerticalMainRoadPadding,
            LevelHeight - 1 - VerticalMainRoadPadding));

        roadMap[startPoint.x, startPoint.y] = true;
        roadMap[endPoint.x, endPoint.y] = true;

        //grab[TurnCount] random points from the map (not from start or end)
        var turnPoints = new List<Vector2Int>();
        var turnPointCount = Mathf.Min(TurnCount, LevelWidth - 3);

        var turnPointPossibleXs = new List<int>();

        for(var i = 1; i < LevelWidth - 2; i++)
        {
            turnPointPossibleXs.Add(i);
        }

        //shuffle possible Xs
        if(turnPointCount > 1)
        {
            for (var i = 0; i < turnPointPossibleXs.Count * 2; i++)
            {
                var r = UnityEngine.Random.Range(1, turnPointPossibleXs.Count);

                var swap = turnPointPossibleXs[r];
                turnPointPossibleXs[r] = turnPointPossibleXs[0];
                turnPointPossibleXs[0] = swap;
            }
        }

        for (var i = 0; i < turnPointCount; i++)
        {
            var x = turnPointPossibleXs[0];
            turnPointPossibleXs.RemoveAt(0);

            var y = UnityEngine.Random.Range(0, LevelHeight - VerticalMainRoadPadding * 2) + VerticalMainRoadPadding;

            if (turnPoints.Count == 0)
                turnPoints.Add(new Vector2Int(x, y));

            else
            {
                var inserted = false;
                for (var j = 0; j < turnPoints.Count; j++)
                {
                    if (turnPoints[j].x > x)
                    {
                        turnPoints.Insert(j, new Vector2Int(x, y));
                        inserted = true;
                        break;
                    }
                }

                if (!inserted) turnPoints.Add(new Vector2Int(x, y));
            }

        }

        //generate a path from start->point0-> ... -> pointx->end
        var crawlX = startPoint.x;
        var crawlY = startPoint.y;

        var branchables = new List<Vector2Int>();

        var lastHoriz = true;
        var horiz = false;
        var lastPoint = new Vector2Int();

        while(crawlY != endPoint.y || crawlX != endPoint.x)
        {
            roadMap[crawlX, crawlY] = true;

            lastPoint = new Vector2Int(crawlX, crawlY);

            if (turnPoints.Count > 0 && crawlX == turnPoints[0].x && crawlY == turnPoints[0].y)
            {
                turnPoints.RemoveAt(0);
            }

            if(turnPoints.Count > 0 && turnPoints[0].x != crawlX)
            {
                if(turnPoints[0].x < crawlX)
                {
                    Debug.LogError("Crawled past turn point, wierd");
                    turnPoints.RemoveAt(0);
                }

                crawlX++;
                horiz = true;
            }
            else if(turnPoints.Count > 0 && turnPoints[0].y != crawlY)
            {
                if(turnPoints[0].y > crawlY)
                {
                    crawlY++;
                }
                else
                {
                    crawlY--;
                }
                horiz = false;
            }
            else if(turnPoints.Count == 0 && crawlX == (endPoint.x - 1) && endPoint.y != crawlY)
            {
                if(endPoint.y > crawlY)
                {
                    crawlY++;
                }
                else
                {
                    crawlY--;
                }
                horiz = false;
            }
            else
            {
                crawlX++;
                horiz = true;
            }

            if(horiz == lastHoriz && crawlX > 1 && crawlX < (LevelWidth - 1))
            {
                branchables.Add(lastPoint);
            }

            lastHoriz = horiz;
        }

        //build branches
        foreach(var branch in branchables)
        {
            //pick a point on the path & branch direction
            var branching = BranchLikelihood > UnityEngine.Random.value;

            if (branching)
            {
                BuildBranch(branch, false);
            }

            branching = BranchLikelihood > UnityEngine.Random.value;

            if (branching)
            {
                BuildBranch(branch, true);
            }
        }

        yield return null;

        //use the bool map to generate roads based on basic logic
        for(var i = 0; i < LevelWidth; i++)
        {
            for(var j = 0; j < LevelHeight; j++)
            {
                bool left = i == 0 ? false : roadMap[i - 1, j];
                bool right = i == (LevelWidth - 1) ? false : roadMap[i + 1, j];
                bool down = j == 0 ? false : roadMap[i, j - 1];
                bool up = j == (LevelHeight - 1) ? false : roadMap[i, j + 1];

                CreateWorldTile(i, j, roadMap[i,j], left, right, down, up);
            }

            yield return null;
        }

        //'grow'[Non Road Growth Count] tiles from the road
        //fill the rest of the grid with impassable terrain

        yield return null;
    }

    private void CreateWorldTile(int i, int j, bool me, bool left, bool right, bool down, bool up)
    {
        var position = new Vector3(i * TileWidth, 0, j * TileWidth) + transform.position;

        //nothing
        if (!me)
        {
            map.Add(Instantiate(WorldTiles[5], position, Quaternion.identity, transform));
        }
        //Intersection
        else if (left && right && up && down)
        {
            map.Add(Instantiate(WorldTiles[1], position, Quaternion.identity, transform));
        }
        //T Junctions
        else if (left && right && up)
        {
            map.Add(Instantiate(WorldTiles[4], position, Quaternion.identity, transform));
        }
        else if (left && right && down)
        {
            map.Add(Instantiate(WorldTiles[4], position, Quaternion.Euler(0, 180, 0), transform));
        }
        else if (up && down && right)
        {
            map.Add(Instantiate(WorldTiles[4], position, Quaternion.Euler(0, 90, 0), transform));
        }
        else if (up && down && left)
        {
            map.Add(Instantiate(WorldTiles[4], position, Quaternion.Euler(0, -90, 0), transform));
        }
        //Straights
        else if (left && right)
        {
            map.Add(Instantiate(WorldTiles[2], position, Quaternion.identity, transform));
        }
        else if (up && down)
        {
            map.Add(Instantiate(WorldTiles[2], position, Quaternion.Euler(0, 90, 0), transform));
        }
        //Turns
        else if (left && down)
        {
            map.Add(Instantiate(WorldTiles[3], position, Quaternion.identity, transform));
        }
        else if (down && right)
        {
            map.Add(Instantiate(WorldTiles[3], position, Quaternion.Euler(0, -90, 0), transform));
        }
        else if (right && up)
        {
            map.Add(Instantiate(WorldTiles[3], position, Quaternion.Euler(0, 180, 0), transform));
        }
        else if (up && left)
        {
            map.Add(Instantiate(WorldTiles[3], position, Quaternion.Euler(0, 90, 0), transform));
        }
        //Dead Ends
        else if (left)
        {
            map.Add(Instantiate(WorldTiles[0], position, Quaternion.Euler(0, 180, 0), transform));
        }
        else if (right)
        {
            map.Add(Instantiate(WorldTiles[0], position, Quaternion.identity, transform));
        }
        else if (down)
        {
            map.Add(Instantiate(WorldTiles[0], position, Quaternion.Euler(0, 90, 0), transform));
        }
        else if (up)
        {
            map.Add(Instantiate(WorldTiles[0], position, Quaternion.Euler(0, -90, 0), transform));
        }
        //nothing
        else
        {
            map.Add(Instantiate(WorldTiles[5], position, Quaternion.identity, transform));
        }
    }

    //crawl randomly until another path or the end of the map is hit, placing trues along the way
    private void BuildBranch(Vector2Int start, bool startDirection)
    {
        //the branch is vertical if the road is horizontal
        var verticalBranch = (start.x > 0 && roadMap[start.x - 1, start.y]) || (start.x < (LevelWidth - 1) && roadMap[start.x + 1, start.y]);

        var startDirectionInt = startDirection ? 1 : -1;

        var crawlX = start.x + (verticalBranch ? 0 : startDirectionInt);
        var crawlY = start.y + (verticalBranch ? startDirectionInt : 0);

        var directions = new Vector2Int[3];

        if (verticalBranch)
        {
            directions[0] = new Vector2Int(-startDirectionInt, 0);
            directions[1] = new Vector2Int(0, startDirectionInt);
            directions[2] = new Vector2Int(startDirectionInt, 0);
        }
        else
        {
            directions[0] = new Vector2Int(0, -startDirectionInt);
            directions[1] = new Vector2Int(startDirectionInt, 0);
            directions[2] = new Vector2Int(0, startDirectionInt);
        }

        for(var i = 0; i < MaxBranchDepth; i++)
        {
            if (crawlX < 1 || crawlX > (LevelWidth - 2) || crawlY < 0 || crawlY > (LevelHeight - 1) || roadMap[crawlX, crawlY])
            {
                //we reached another road, or the edge of the map
                break;
            }

            roadMap[crawlX, crawlY] = true;

            var nextDir = UnityEngine.Random.Range(0, 3);

            if(i == 0)
            {
                var extraChance = UnityEngine.Random.Range(0, 3);
                nextDir = extraChance == 1 ? 1 : nextDir;
            }

            crawlX += directions[nextDir].x;
            crawlY += directions[nextDir].y;
        }
    }
}
