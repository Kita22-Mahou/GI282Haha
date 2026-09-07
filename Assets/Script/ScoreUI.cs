using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    [Header("High Score")]
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            Refresh(
                GameManager.Instance.Score,
                GameManager.Instance.HighScore
            );
        }
    }

    public void Refresh(int score, int highScore)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("N0");
        }

        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString("N0");
        }
    }
}