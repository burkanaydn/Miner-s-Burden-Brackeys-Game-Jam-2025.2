using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText; // Sayaç yazýsý
    [SerializeField] private GameObject gameOverPanel;  // Game Over paneli
    [SerializeField] private GameObject timerPanel;  // Timer paneli
    [SerializeField] private float gameDuration = 480f; // 8 dakika = 480 saniye

    private float currentTime;
    private bool isGameOver = false;

    private void Start()
    {
        currentTime = gameDuration;
        gameOverPanel.SetActive(false);

        if(timerPanel != null)
        DoTweenUIManager.Instance.PlayAttentionLoopPop(timerPanel);
    }

    private void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
