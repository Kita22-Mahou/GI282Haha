using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box Data")]
    [SerializeField]
    private int tier = 1;

    [SerializeField]
    private string variant = "A";

    [Header("References")]
    public BoxDatabase database;

    private Rigidbody2D rb;
    private bool isMerging = false;
    private float mergeLockTimer;

    public int Tier => tier;
    public string Variant => variant;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (database == null)
            database = FindFirstObjectByType<BoxDatabase>();

        mergeLockTimer = 0.08f;
    }

    private void Update()
    {
        if (mergeLockTimer > 0f)
            mergeLockTimer -= Time.deltaTime;
    }

    // =========================================
    // ตั้งค่า Box
    // =========================================

    public void SetBoxData(
        int newTier,
        string newVariant
    )
    {
        tier = Mathf.Clamp(
            newTier,
            1,
            6
        );

        variant =
            string.IsNullOrWhiteSpace(newVariant)
                ? "A"
                : newVariant.Trim().ToUpperInvariant();
    }

    // =========================================
    // Collision
    // =========================================

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (isMerging)
            return;

        if (mergeLockTimer > 0f)
            return;

        Box other =
            collision.gameObject.GetComponent<Box>();

        if (other == null ||
            other == this)
            return;

        if (other.isMerging)
            return;

        if (other.mergeLockTimer > 0f)
            return;

        // ต้อง Tier เดียวกัน
        if (tier != other.tier)
            return;

        // ต้อง Variant เดียวกัน
        if (variant != other.variant)
            return;

        // Tier 6 สูงสุด
        if (tier >= 6)
            return;

        Merge(other);
    }

    // =========================================
    // Merge
    // =========================================

    private void Merge(Box other)
    {
        isMerging = true;
        other.isMerging = true;

        Vector3 mergePosition =
            (
                transform.position +
                other.transform.position
            ) * 0.5f;

        int nextTier =
            tier + 1;

        string nextVariant =
            variant;

        if (database == null)
            database =
                FindFirstObjectByType<BoxDatabase>();

        if (database == null)
        {
            Debug.LogError(
                "Box: BoxDatabase not found."
            );

            return;
        }

        GameObject nextPrefab =
            database.GetPrefab(
                nextTier,
                nextVariant
            );

        if (nextPrefab == null)
        {
            Debug.LogError(
                $"Box: Missing prefab for Tier {nextTier}, Variant {nextVariant}."
            );

            isMerging = false;
            other.isMerging = false;

            return;
        }

        GameObject newBoxObject =
            Instantiate(
                nextPrefab,
                mergePosition,
                Quaternion.identity
            );

        Box newBox =
            newBoxObject.GetComponent<Box>();

        if (newBox != null)
        {
            newBox.database = database;

            newBox.SetBoxData(
                nextTier,
                nextVariant
            );
        }

        Rigidbody2D newRb =
            newBoxObject.GetComponent<Rigidbody2D>();

        if (newRb != null)
        {
            newRb.simulated = true;
            newRb.linearVelocity = Vector2.zero;
            newRb.angularVelocity = 0f;
        }

        // Score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(
                tier
            );
        }

        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}