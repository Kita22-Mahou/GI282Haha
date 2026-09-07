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

    public Slider Slider;

    private void Start()
    {
        overloadText.enabled = false;
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
                SceneManager.LoadScene("Start Scene");
            }

            if (time >= 3)
            {
                Slider.value += 0.175f;
                overloadText.enabled = true;
            }
            else
            {
                Slider.value = 0;
                overloadText.enabled = false;
            }
        }
        else
        {
            time = 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
