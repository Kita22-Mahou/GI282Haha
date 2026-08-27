using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        if (GameManager.Instance != null)
            Refresh(GameManager.Instance.Score);
    }

    public void Refresh(int score)
    {
        if (scoreText == null)
        {
            Debug.LogWarning("ScoreUI: Assign Score Text in the Inspector.");
            return;
        }

        scoreText.text = score.ToString("N0");
    }
}
