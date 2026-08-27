using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score = 0;

    public ScoreUI scoreUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();

        Debug.Log("GameManager Ready!");
    }

    public void AddScore(int amount)
    {
        score += amount;

        Debug.Log(
            "ADD SCORE: +" +
            amount +
            " | TOTAL: " +
            score
        );

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreUI == null)
        {
            Debug.LogWarning(
                "ScoreUI is NOT connected!"
            );

            return;
        }

        scoreUI.UpdateScore();
    }
}