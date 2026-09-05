using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public bool isCollided = false;

    public float loseTime = 20;
    public float time = 0;

    public Vector2 boxSize;
    public LayerMask itemLayer;

    public TextMeshProUGUI overloadText;

    private void Start()
    {
        overloadText.enabled = false;
    }
    private void Update()
    {
        Collider2D[] items = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, itemLayer);

        isCollided = items.Length > 0;

        if(isCollided)
        {
            time += Time.deltaTime;
            if (time >= loseTime)
            {
                SceneManager.LoadScene("Start Scene");
            }

            if (time >= 3)
            {
                overloadText.enabled = true;
            }
            else
            {
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
