using UnityEngine;

public class TimeManager : MonoBehaviour
{
    internal void SetUp()
    {
        Time.timeScale = 1f;
    }
    
    internal void PasueGame()
    {
        Time.timeScale = 0f;
    }

    internal void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
