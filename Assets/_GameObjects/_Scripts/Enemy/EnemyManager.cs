using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Types")] 
    [SerializeField] private List<string> enemyTypeTags;
    
    [Header("Spike Types")] 
    [SerializeField] private List<string> spikeTypeTags;

    #region SingleTon
    public static EnemyManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    internal string GetRandomEnemyType()
    {
        int randType = Random.Range(0, enemyTypeTags.Count);
        return enemyTypeTags[randType];
    }
    
    internal string GetRandomSpikeType()
    {
        int randType = Random.Range(0, spikeTypeTags.Count);
        return spikeTypeTags[randType];
    }
}
