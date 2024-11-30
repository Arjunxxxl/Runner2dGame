using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State")] 
    [SerializeField] private GameState gameState;
    private float stateDuration;
    private float stateTimer;
    
    [Header("Player")] 
    [SerializeField] private Player player;
    [SerializeField] private Vector3 playerStartPos;
    
    [Header("Tile Generator")]
    [SerializeField] private TileGenerator tileGenerator;

    [Header("Camera")] 
    [SerializeField] private CameraController cameraController;
    
    [Header("Time Manager")]
    [SerializeField] private TimeManager timeManager;

    [Header("UI")] 
    [SerializeField] private UiManager uiManager;

    public static Action StartGame;
    public static Action GameCompleted;
    public static Action PlayerDied;
    
    #region SingleTon
    public static GameManager Instance;
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUpGame();
        SetGameState(GameState.MainMenu);
    }

    private void Update()
    {
        UpdateStateTimer();
    }

    #region SetUp
    
    private void SetUpGame()
    {
        timeManager.SetUp();
        
        LocalDataManager.Instance.LoadData();
        uiManager.SetUp();
        
        player.gameObject.SetActive(true);
        player.transform.position = playerStartPos;
        player.SetUp();
        
        tileGenerator.SetUp();
        cameraController.SetUp(player);
    }

    #endregion
    
    #region Game State

    internal void SetGameState(GameState newState)
    {
        if (gameState == newState)
        {
            return;
        }

        if (gameState == GameState.Pause)
        {
            timeManager.ResumeGame();
        }
        
        if (newState == GameState.Pause)
        {
            timeManager.PasueGame();
        }
        else if (newState == GameState.WaitingForGameOver)
        {
            stateDuration = Constants.GameOverWaitDelay;
        }

        stateTimer = 0;
        gameState = newState;

        uiManager.EnableMenu(gameState);
    }

    private void UpdateStateTimer()
    {
        if (gameState == GameState.WaitingForGameOver)
        {
            stateTimer += Time.deltaTime;

            if (stateTimer >= stateDuration)
            {
                SetGameState(GameState.GameOver);
            }
        }
    }

    internal GameState GetGameState()
    {
        return gameState;
    }

    #endregion

    #region Scene

    internal void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}
