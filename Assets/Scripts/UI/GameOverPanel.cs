using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TMP namespace'i ekle

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text scoreText; // Text -> TMP_Text

    private void OnEnable()
    {
        UpdateScore();

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);
    }

    private void OnDisable()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartScene);
    }

    private void UpdateScore()
    {
        if (scoreText != null && EconomyManager.Instance != null)
        {
            scoreText.text = "Score: " + EconomyManager.Instance.CurrentMoney;
        }
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
