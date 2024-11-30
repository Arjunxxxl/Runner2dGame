using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private Button saveButton;

    private void Start()
    {
        saveButton.onClick.AddListener(OnClickSaveButton);
    }

    internal void SetUp()
    {
        int difficulty = LocalDataManager.Instance.GetDifficulty();
        difficultySlider.minValue = 1;
        difficultySlider.maxValue = Constants.MaxDifficultyValue;
        difficultySlider.value = difficulty;
    }

    private void OnClickSaveButton()
    {
        SoundManager.Instance.PlayAudio("button");
        LocalDataManager.Instance.SetDifficulty((int)difficultySlider.value);
        GameManager.Instance.ReloadScene();
    }
}
