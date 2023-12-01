using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    public static MapStatus status;

    public List<VoxelTile> tilePrefabs;
    public Vector2Int mapSize = new Vector2Int(10, 10);
    public VoxelTile anomalyTile;
    public float placeDelay = 0f;

    private VoxelTile[,] spawnedTiles;

    void Start()
    {
        status = MapStatus.Empty;
        spawnedTiles = new VoxelTile[mapSize.x, mapSize.y];

        tilePrefabs = new List<VoxelTile>();
        FillTilePrefabs();


        foreach (VoxelTile tilePrefab in tilePrefabs)
        {
            tilePrefab.CalculateSidesColors();
        }
        StartCoroutine(DefaultGenerate());
    }

    private void Update()
    {
        if (status == MapStatus.Loaded && GameObject.Find("Tiles"))
        {
            Destroy(GameObject.Find("Tiles"));
        }
    }

    void FillTilePrefabs()
    {
        GameObject tilesParent = GameObject.Find("Tiles");

        if (tilesParent != null)
        {
            foreach (Transform child in tilesParent.transform)
            {
                VoxelTile voxelTile = child.GetComponent<VoxelTile>();
                if (voxelTile != null)
                {
                    tilePrefabs.Add(voxelTile);
                }
            }
        }
    }

    private IEnumerator DefaultGenerate()
    {
        status = MapStatus.Loading;
        for (int x = 2; x < mapSize.x - 2; x++)
        {
            for (int y = 2; y < mapSize.y - 2; y++)
            {
                yield return new WaitForSeconds(placeDelay);
                PlaceTile(x, y);
            }
        }
        AnomalyPlacer();
        status = MapStatus.Loaded;
    }

    private IEnumerator RadialGenerate() 
    {
        status = MapStatus.Loading;
        int centerX = mapSize.x / 2;
        int centerY = mapSize.y / 2;

        yield return new WaitForSeconds(placeDelay);
        PlaceTile(centerX, centerY);

        int maxRadius = Math.Max(mapSize.x, mapSize.y) / 2;

        for (int r = 1; r <= maxRadius; r++)
        {
            for (int dx = -r; dx <= r; dx++)
            {
                for (int dy = -r; dy <= r; dy++)
                {
                    if (Math.Abs(dx) != r && Math.Abs(dy) != r) continue;

                    int x = centerX + dx;
                    int y = centerY + dy;

                    if (x >= 1 && x < mapSize.x - 1 && y >= 1 && y < mapSize.y - 1)
                    {
                        yield return new WaitForSeconds(placeDelay);
                        PlaceTile(x, y);
                    }
                }
            }
        }
        AnomalyPlacer();
        status = MapStatus.Loaded;
    }

    private void AnomalyPlacer() 
    {
        for (int x = 1; x < mapSize.x - 1; x++)
        {
            for (int y = 1; y < mapSize.y - 1; y++)
            {
                if (spawnedTiles[x, y] == null)
                {
                    spawnedTiles[x, y] = Instantiate(anomalyTile, new Vector3(x, 0, y) * 1.6f, anomalyTile.transform.rotation);
                }
            }
        }
    }

    private void PlaceTile(int x, int y) 
    {
        List<VoxelTile> availibleTiles = new List<VoxelTile>();

        foreach (VoxelTile tilePrefab in tilePrefabs) 
        {
            if (CanAppendTile(spawnedTiles[x - 1, y], tilePrefab, Vector3.left) &&
                CanAppendTile(spawnedTiles[x + 1, y], tilePrefab, Vector3.right) &&
                CanAppendTile(spawnedTiles[x, y - 1], tilePrefab, Vector3.back) &&
                CanAppendTile(spawnedTiles[x, y + 1], tilePrefab, Vector3.forward))
            {
                availibleTiles.Add(tilePrefab);
            }
        }

        if (availibleTiles.Count == 0) return;

        VoxelTile selectedTile = GetRandomTile(availibleTiles);
        spawnedTiles[x,y] =  Instantiate(selectedTile, new Vector3(x, 0, y) * 1.6f, selectedTile.transform.rotation);
    }

    private VoxelTile GetRandomTile(List<VoxelTile> availibleTiles) 
    {
        List<float> chances = new List<float>();
        for(int i = 0; i < availibleTiles.Count; i++) 
        {
            chances.Add(availibleTiles[i].Weight);
        }

        float value = UnityEngine.Random.Range(0, chances.Sum());
        float sum = 0;

        for(int i = 0; i < chances.Count; i++) 
        {
            sum += chances[i];
            if (value < sum) return availibleTiles[i];
        }

        return availibleTiles[availibleTiles.Count - 1];
    }

    private bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Vector3 direction)
    {
        if (existingTile == null) return true;

        if (direction == Vector3.right) 
        {
            return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);   
        }
        else if (direction == Vector3.left) 
        {
            return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);
        }
        else if (direction == Vector3.forward)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);
        }
        else if (direction == Vector3.back)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);
        }
        else
        {
            throw new System.Exception();
        }
    }
}
