using UnityEngine;

public class LocalDataManager : MonoBehaviour
{
    [Header("Difficulty")]
    private static readonly string DifficultyTag = "Difficulty";
    [SerializeField] private int difficulty;
    
    #region SingleTon
    public static LocalDataManager Instance;
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

    internal void LoadData()
    {
        LoadDifficulty();
    }
    
    #region Difficulty

    private void LoadDifficulty()
    {
        difficulty = PlayerPrefs.GetInt(DifficultyTag, 1);
    }

    internal int GetDifficulty()
    {
        return difficulty;
    }

    internal void SetDifficulty(int val)
    {
        difficulty = val;
        
        if (difficulty > Constants.MaxDifficultyValue)
        {
            difficulty = Constants.MaxDifficultyValue;
        } 
        
        PlayerPrefs.SetInt(DifficultyTag, difficulty);
    }
    
    #endregion
}
