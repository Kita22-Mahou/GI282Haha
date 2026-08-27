using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box")]
    public int level = 1;

    [Header("Configuration")]
    public BoxConfig config;

    private bool isMerging = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        ApplyLevelData();
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;

        ApplyLevelData();
    }

    void ApplyLevelData()
    {
        if (config == null)
        {
            Debug.LogWarning("BoxConfig is missing!");
            return;
        }

        BoxConfig.LevelData data =
            config.GetLevel(level);

        if (data == null)
            return;

        // ปรับขนาด
        transform.localScale =
            new Vector3(
                data.size.x,
                data.size.y,
                1f
            );

        // ปรับน้ำหนัก
        if (rb != null)
        {
            rb.mass = data.mass;
        }

        // เปลี่ยน Sprite
if (spriteRenderer != null)
{
    spriteRenderer.color = data.color;
}
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (isMerging)
            return;

        Box other =
            collision.gameObject.GetComponent<Box>();

        if (other == null)
            return;

        if (other.isMerging)
            return;

        // ต้อง Level เดียวกัน
        if (level != other.level)
            return;

        // Level 8 เป็น Level สูงสุด
        if (level >= 8)
            return;

        Merge(other);
    }

    private void Merge(Box other)
{
    // ป้องกัน Merge ซ้ำ
    isMerging = true;
    other.isMerging = true;

    // =========================
    // เพิ่ม Score
    // =========================

    if (GameManager.Instance != null)
    {
        BoxConfig.LevelData data =
            config.GetLevel(level);

        if (data != null)
        {
            GameManager.Instance.AddScore(data.score);
        }
    }
    else
    {
        Debug.LogError("GameManager.Instance is NULL!");
    }

    // =========================
    // ตำแหน่ง Merge
    // =========================

    Vector3 mergePosition =
        (transform.position +
         other.transform.position) / 2f;

    int nextLevel = level + 1;

    // =========================
    // สร้างกล่องใหม่
    // =========================

    GameObject newBoxObject =
        Instantiate(
            gameObject,
            mergePosition,
            Quaternion.identity
        );

    Box newBox =
        newBoxObject.GetComponent<Box>();

    newBox.level = nextLevel;
    newBox.config = config;

    newBox.ApplyLevelData();

    // =========================
    // Reset Physics
    // =========================

    Rigidbody2D newRb =
        newBox.GetComponent<Rigidbody2D>();

    if (newRb != null)
    {
        newRb.velocity = Vector2.zero;
        newRb.angularVelocity = 0f;
    }

    // =========================
    // ลบกล่องเก่า
    // =========================

    Destroy(gameObject);
    Destroy(other.gameObject);
}
}