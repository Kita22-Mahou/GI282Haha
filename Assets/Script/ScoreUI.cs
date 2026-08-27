using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager.Instance is NULL");
            return;
        }

        if (scoreText == null)
        {
            Debug.LogWarning("Score Text is NOT assigned!");
            return;
        }

        scoreText.text =
            "SCORE " +
            GameManager.Instance.score.ToString();
    }
}