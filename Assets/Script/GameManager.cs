using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private int score;
    [SerializeField] private ScoreUI scoreUI;

    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        RefreshScoreUI();
    }

    public void AddScore(int amount)
    {
        if (amount < 0)
            return;

        score += amount;
        RefreshScoreUI();
        Debug.Log($"Score +{amount} = {score}");
    }

    public void ResetScore()
    {
        score = 0;
        RefreshScoreUI();
    }

    private void RefreshScoreUI()
    {
        if (scoreUI != null)
            scoreUI.Refresh(score);
    }
}
