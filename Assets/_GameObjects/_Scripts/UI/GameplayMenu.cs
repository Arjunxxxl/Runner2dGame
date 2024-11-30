using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMenu : MonoBehaviour
{
    [Header("Hp")]
    [SerializeField] private List<Image> hpIcons;
    [SerializeField] private Color hpIconActive;
    [SerializeField] private Color hpIconInactive;

    [Header("Button")]
    [SerializeField] private Button pauseButton;
    
    public static Action<int> UpdateHpUi;

    private void OnEnable()
    {
        UpdateHpUi += UpdateHp;
    }

    private void OnDisable()
    {
        UpdateHpUi -= UpdateHp;
    }

    private void Start()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
    }

    #region SetUp

    internal void SetUp()
    {
        for (int i = 0; i < hpIcons.Count; i++)
        {
            hpIcons[i].color = hpIconActive;
        }
    }

    #endregion

    #region Hp

    private void UpdateHp(int hpLeft)
    {
        for (int i = 0; i < hpIcons.Count; i++)
        {
            hpIcons[i].color = i < hpLeft ? hpIconActive : hpIconInactive;
        }
    }

    #endregion

    #region Button

    private void OnClickPauseButton()
    {
        SoundManager.Instance.PlayAudio("button");
        GameManager.Instance.SetGameState(GameState.Pause);
    }

    #endregion
}
