using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public bool isCollided = false;

    public float loseTime = 20;
    public float time = 0;

    public Vector2 boxSize;
    public LayerMask itemLayer;

    public TextMeshProUGUI overloadText;

    public Scrollbar overloadBar;

    public GameoverScreen gameoverScreen;

    private void Start()
    {
        overloadText.enabled = false;
        gameoverScreen = FindAnyObjectByType<GameoverScreen>();
    }

    private void FixedUpdate()
    {
        Collider2D[] items = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, itemLayer);

        isCollided = items.Length > 0;

        if (isCollided)
        {
            time += Time.deltaTime;
            if (time >= loseTime)
            {
                gameoverScreen.MakeGameOver(true);

                Invoke(nameof(TimeStop),2);
            }

            if (time >= 3)
            {
                overloadBar.value += 0.00175f;
                overloadText.enabled = true;
            }
            else
            {
                overloadBar.value = 0;
                overloadText.enabled = false;
            }
        }
        else
        {
            time = 0f;
        }
    }

    private void TimeStop()
    {
        Time.timeScale = 0f;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
