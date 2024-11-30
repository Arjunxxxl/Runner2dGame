using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button homeButton; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        homeButton.onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickHomeButton()
    {
        SoundManager.Instance.PlayAudio("button");
        GameManager.Instance.ReloadScene();
    }
}
