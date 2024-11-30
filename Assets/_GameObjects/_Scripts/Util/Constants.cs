using UnityEngine;

public static class Constants
{
    //Enemy Spawning
    public static readonly Vector2Int MaxEnemyCountPerTile = new Vector2Int(2, 6);
    public static readonly Vector2Int MaxSpikeCountPerTile = new Vector2Int(1, 5);
    public static readonly float ChanceForNoEnemyTile = 0.075f;
    
    //Enemy Movement
    public static readonly Vector2 EnemyMoveSpeedVariation = new Vector2(0.0f, 0.5f);
    public static readonly Vector2 EnemyChaseWaitDurationRange = new Vector2(0.25f, 1.0f);
    
    //Player Collision
    public static readonly float PlayerYOffsetForEnemyKill = 0.6f;
    
    //Player Hp
    public static readonly int MaxPlayerHp = 5;
    
    //Player Death
    public static readonly float PlayerYPosForFallDeath = -5f;
    
    //Difficulty
    public static readonly int MaxDifficultyValue = 3;
    
    //States
    public static readonly float GameOverWaitDelay = 0.75f;
    
    //Colliders
    public static readonly int LevelCompleteObjLayer = 10;
}
