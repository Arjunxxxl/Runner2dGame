using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeButton.onClick.AddListener(OnClickResumeButton);
    }

    private void OnClickResumeButton()
    {
        SoundManager.Instance.PlayAudio("button");
        GameManager.Instance.SetGameState(GameState.Gameplay);
    }
}
