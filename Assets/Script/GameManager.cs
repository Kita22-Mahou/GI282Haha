using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private int score;
    [SerializeField] private ScoreUI scoreUI;

    private int highScore;

    public int Score => score;
    public int HighScore => highScore;

    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // โหลดคะแนนสูงสุดที่เคยบันทึกไว้
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
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

        // ตรวจสอบว่าคะแนนสูงกว่าไหม
        if (score > highScore)
        {
            highScore = score;

            // บันทึก High Score
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        RefreshScoreUI();

        Debug.Log(
            $"Score +{amount} = {score} | High Score = {highScore}"
        );
    }

    public void ResetScore()
    {
        // รีเซ็ตเฉพาะคะแนนปัจจุบัน
        score = 0;

        // High Score
        RefreshScoreUI();
    }

    private void RefreshScoreUI()
    {
        if (scoreUI != null)
        {
            scoreUI.Refresh(
                score,
                highScore
            );
        }
    }

    // ใช้สำหรับล้าง High Score ถ้าต้องการ
    public void ResetHighScore()
    {
        highScore = 0;

        PlayerPrefs.SetInt(HighScoreKey, 0);
        PlayerPrefs.Save();

        Debug.Log("High Score Reset");
    }
}