using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
        settingButton.onClick.AddListener(OnClickSettingsButton);
    }

    private void OnClickPlayButton()
    {
        SoundManager.Instance.PlayAudio("button");
        GameManager.Instance.SetGameState(GameState.Gameplay);
    }

    private void OnClickSettingsButton()
    {
        SoundManager.Instance.PlayAudio("button");
        GameManager.Instance.SetGameState(GameState.Settings);
    }
}
