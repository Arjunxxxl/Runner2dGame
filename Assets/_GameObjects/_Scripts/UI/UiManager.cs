using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("Canvases")] 
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Canvas gameplayCanvas;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas gameoverCanvas;
    [SerializeField] private Canvas gameCompleteCanvas;

    [Header("REf")] 
    [SerializeField] private SettingMenu settingMenu;
    [SerializeField] private GameplayMenu gameplayMenu;

    internal void SetUp()
    {
        settingMenu.SetUp();
        gameplayMenu.SetUp();
    }
    
    internal void EnableMenu(GameState gameState)
    {
        mainMenuCanvas.enabled = gameState == GameState.MainMenu;
        settingsCanvas.enabled = gameState == GameState.Settings;
        gameplayCanvas.enabled = gameState == GameState.Gameplay;
        pauseCanvas.enabled = gameState == GameState.Pause;
        gameoverCanvas.enabled = gameState == GameState.GameOver;
        gameCompleteCanvas.enabled = gameState == GameState.GameComplete;
    }
}
