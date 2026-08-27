using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box Data")]
    [SerializeField] private int level = 1;

    [Header("References")]
    public BoxDatabase database;

    private Rigidbody2D rb;
    private bool isMerging;
    private float mergeLockTimer;

    public int Level => level;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (database == null)
            database = FindFirstObjectByType<BoxDatabase>();

        mergeLockTimer = 0.08f;
    }

    private void Update()
    {
        if (mergeLockTimer > 0f)
            mergeLockTimer -= Time.deltaTime;
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Clamp(newLevel, 1, 8);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerging || mergeLockTimer > 0f)
            return;

        Box other = collision.gameObject.GetComponent<Box>();

        if (other == null || other == this)
            return;

        if (other.isMerging || other.mergeLockTimer > 0f)
            return;

        if (level != other.level)
            return;

        if (level >= 8)
            return;

        Merge(other);
    }

    private void Merge(Box other)
    {
        isMerging = true;
        other.isMerging = true;

        Vector3 mergePosition =
            (transform.position + other.transform.position) * 0.5f;

        int nextLevel = level + 1;

        if (database == null)
            database = FindFirstObjectByType<BoxDatabase>();

        GameObject nextPrefab = database != null
            ? database.GetPrefab(nextLevel)
            : null;

        if (nextPrefab == null)
        {
            Debug.LogError($"Box: Missing prefab for Level {nextLevel}.");
            isMerging = false;
            other.isMerging = false;
            return;
        }

        GameObject newBoxObject = Instantiate(
            nextPrefab,
            mergePosition,
            Quaternion.identity
        );

        Box newBox = newBoxObject.GetComponent<Box>();
        if (newBox != null)
        {
            newBox.database = database;
            newBox.SetLevel(nextLevel);
        }

        Rigidbody2D newRb = newBoxObject.GetComponent<Rigidbody2D>();
        if (newRb != null)
        {
            newRb.simulated = true;
            newRb.velocity = Vector2.zero;
            newRb.angularVelocity = 0f;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(level);

        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}
