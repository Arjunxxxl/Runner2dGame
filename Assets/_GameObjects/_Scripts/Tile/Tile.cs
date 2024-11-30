using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    [Header("Tile Data")] 
    [SerializeField] private float tileLength;

    [Header("Collider")] 
    [SerializeField] private TilemapCollider2D tilemapCollider2D;

    [Header("Enemy Data")] 
    [SerializeField] private List<TileEnemyData> tileEnemiesData;
    [SerializeField] private List<Transform> spikesT;
    [SerializeField] private Vector2 patrolDurationRange;
    [SerializeField] private Vector2 idleDurationRange;
    
    internal void SetUp()
    {
        tilemapCollider2D.compositeOperation = Collider2D.CompositeOperation.Merge;
        tilemapCollider2D.ProcessTilemapChanges();
    }

    #region Enemy

    internal void SpawnEnemies()
    {
        int difficulty = LocalDataManager.Instance.GetDifficulty();
        int minEnemiesCt = Constants.MaxEnemyCountPerTile.x;
        int maxEnemiesCt = Constants.MaxEnemyCountPerTile.y;
        int enemiesToSpawn = minEnemiesCt + GetMaxEnemyCtBasedOnDifficulty(difficulty, maxEnemiesCt - minEnemiesCt);
        
        if (enemiesToSpawn > tileEnemiesData.Count)
        {
            enemiesToSpawn = Random.Range(1, tileEnemiesData.Count);
        }

        if (Random.value < Constants.ChanceForNoEnemyTile)
        {
            enemiesToSpawn = 0;
            return;
        }
        
        List<TileEnemyData> enemyData = GetRandomEnemies(tileEnemiesData, enemiesToSpawn);
        
        for (int i = 0; i < enemyData.Count; i++)
        {
            TileEnemyData tileEnemyData = enemyData[i];
            Vector3 spawnPos = tileEnemyData.enemySpawnT.transform.position;

            string randEnemyTag = EnemyManager.Instance.GetRandomEnemyType();
            GameObject enemyObj = ObjectPooler.Instance.SpawnFormPool(randEnemyTag, spawnPos, Quaternion.identity);
            
            enemyObj.SetActive(true);

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.SetUp(tileEnemyData.patrollingPoints, idleDurationRange, patrolDurationRange);
        }
    }

    internal void SpawnSpikes()
    {
        int difficulty = LocalDataManager.Instance.GetDifficulty();
        int minSpikeCt = Constants.MaxSpikeCountPerTile.x;
        int maxSpikeCt = Constants.MaxSpikeCountPerTile.y;
        int spikesToSpawn = minSpikeCt + GetMaxEnemyCtBasedOnDifficulty(difficulty, maxSpikeCt - minSpikeCt);
        
        if (spikesToSpawn > spikesT.Count)
        {
            spikesToSpawn = Random.Range(1, spikesT.Count);
        }

        if (Random.value < Constants.ChanceForNoEnemyTile)
        {
            spikesToSpawn = 0;
            return;
        }
        
        List<Transform> selectedSpikesT = GetRandomSpikeT(spikesT, spikesToSpawn);
        
        for (int i = 0; i < selectedSpikesT.Count; i++)
        {
            Transform spawnT = selectedSpikesT[i];
            Vector3 spawnPos = spawnT.transform.position;

            string randSpikeTag = EnemyManager.Instance.GetRandomSpikeType();
            GameObject spikeObj = ObjectPooler.Instance.SpawnFormPool(randSpikeTag, spawnPos, Quaternion.identity);
            
            spikeObj.SetActive(true);
        }
    }
    
    private int GetMaxEnemyCtBasedOnDifficulty(int difficulty, int maxValue)
    {
        difficulty = Mathf.Clamp(difficulty, 1, Constants.MaxDifficultyValue);

        int enemyCt = Mathf.CeilToInt(maxValue / Constants.MaxDifficultyValue * difficulty);

        return enemyCt;
    }
    
    List<TileEnemyData> GetRandomEnemies(List<TileEnemyData> enemyList, int count)
    {
        List<TileEnemyData> selectedEnemies = new List<TileEnemyData>();
        List<TileEnemyData> tempList = new List<TileEnemyData>(enemyList);

        count = Mathf.Min(count, tempList.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedEnemies.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); 
        }

        return selectedEnemies;
    }

    List<Transform> GetRandomSpikeT(List<Transform> spikesT, int count)
    {
        List<Transform> selectedEnemies = new List<Transform>();
        List<Transform> tempList = new List<Transform>(spikesT);

        count = Mathf.Min(count, tempList.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedEnemies.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); 
        }

        return selectedEnemies;
    }
    
    #endregion
    
    #region Getter/Setter

    internal float GetTileLength()
    {
        return tileLength;
    }

    #endregion
}
