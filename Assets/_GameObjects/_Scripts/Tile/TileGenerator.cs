using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGenerator : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private Vector2Int tileRangePerLevel;
    [SerializeField] private int tileToSpawn;

    [Header("Tile Spawning Data")] 
    [SerializeField] private int totalTileTypes;
    [SerializeField] private Vector3 spawnStarPos;
    [SerializeField] private Vector2 gapBtwTiles;
    [SerializeField] private List<Tile> spawnedTiles;

    [Header("Testing")] 
    [SerializeField] private bool isTestingEnabled;
    [SerializeField] private int overrideTotalTileToSpawn;
    [SerializeField] private int overrideTileTypes;

    private TilePooler tilePooler;
    
    internal void SetUp()
    {
        tilePooler = TilePooler.Instance;
        
        CalcLevelData();
        SpawnTile();
        SpawnTileEnemies();
    }

    #region Level Calculation

    private void CalcLevelData()
    {
        tileToSpawn = Random.Range(tileRangePerLevel.x, tileRangePerLevel.y + 1);

        if (isTestingEnabled)
        {
            tileToSpawn = overrideTotalTileToSpawn;
        }
    }

    #endregion

    #region Spawn Tiles

    private void SpawnTile()
    {
        spawnedTiles = new List<Tile>();
        
        Vector3 spawnPos = spawnStarPos;
        
        for (int i = 0; i < tileToSpawn; i++)
        {
            int randTile = Random.Range(1, totalTileTypes + 1);
            
            if (isTestingEnabled)
            {
                randTile = overrideTileTypes;
            }

            if (i == tileToSpawn - 1)
            {
                randTile = -1;
            }
            
            string tileToSpawnTag = "Tile" + randTile;
            
            GameObject tileGo = tilePooler.SpawnFormPool(tileToSpawnTag, transform.position, Quaternion.identity);
            tileGo.SetActive(true);
            
            Tile tile = tileGo.GetComponent<Tile>();
            tile.SetUp();

            float tileHalfLength = tile.GetTileLength() * 0.5f;
            
            if (i == 0)
            {
                spawnStarPos.x += tileHalfLength + 2;
                tileGo.transform.position = spawnStarPos;
                
                spawnPos = spawnStarPos;
            }
            else
            {
                float lastTileHalfLength = spawnedTiles[spawnedTiles.Count - 1].GetTileLength() * 0.5f;
                float gap = Random.Range(gapBtwTiles.x, gapBtwTiles.y);

                spawnPos.x += (lastTileHalfLength + tileHalfLength + gap);
                
                tileGo.transform.position = spawnPos;
            }

            spawnedTiles.Add(tile);
        }
    }

    #endregion

    #region Enemies

    private void SpawnTileEnemies()
    {
        for (int i = 0; i < spawnedTiles.Count; i++)
        {
            spawnedTiles[i].SpawnEnemies();
            spawnedTiles[i].SpawnSpikes();
        }
    }

    #endregion
}
