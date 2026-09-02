using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box Data")]
    [SerializeField] private int tier = 1;
    [SerializeField] private string variant = "A";

    [Header("References")]
    [SerializeField] private BoxDatabase database;

    [Header("Merge Settings")]
    [SerializeField] private float mergeCheckRadius = 0.15f;
    [SerializeField] private float mergeCooldown = 0.08f;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 6f;
    [SerializeField] private float explosionUpForce = 1.0f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private bool isMerging = false;
    private float mergeTimer = 0f;

    public int Tier => tier;
    public string Variant => variant;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (database == null)
            database = FindAnyObjectByType<BoxDatabase>();

        mergeTimer = mergeCooldown;
    }

    private void Update()
    {
        if (mergeTimer > 0f)
            mergeTimer -= Time.deltaTime;
    }

    // =====================================================
    // Set Data
    // =====================================================

    public void SetBoxData(int newTier, string newVariant)
    {
        tier = Mathf.Clamp(newTier, 1, 6);

        variant = string.IsNullOrWhiteSpace(newVariant)
            ? "A"
            : newVariant.Trim().ToUpperInvariant();
    }

    public void SetDatabase(BoxDatabase newDatabase)
    {
        database = newDatabase;
    }

    // =====================================================
    // Collision
    // =====================================================

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryMerge(collision.gameObject);
    }

    // =====================================================
    // Backup Merge Check
    // =====================================================

    //private void FixedUpdate()
    //{
    //    if (isMerging)
    //        return;

    //    if (mergeTimer > 0f)
    //        return;

    //    CheckNearbyBoxes();
    //}

    private void CheckNearbyBoxes()
    {
        if (boxCollider == null)
            return;

        Bounds bounds = boxCollider.bounds;

        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                bounds.center,
                bounds.size + Vector3.one * mergeCheckRadius,
                0f
            );

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (hit.gameObject == gameObject)
                continue;

            Box other =
                hit.GetComponent<Box>();

            if (other == null)
                continue;

            if (TryMerge(other))
                return;
        }
    }

    // =====================================================
    // Check Merge
    // =====================================================

    private bool TryMerge(GameObject otherObject)
    {
        if (otherObject == null)
            return false;

        Box other =
            otherObject.GetComponent<Box>();

        if (other == null)
            return false;

        return TryMerge(other);
    }

    private bool TryMerge(Box other)
    {
        if (other == null)
            return false;

        if (isMerging || other.isMerging)
            return false;

        if (mergeTimer > 0f ||
            other.mergeTimer > 0f)
            return false;

        if (tier != other.tier)
            return false;

        if (variant != other.variant)
            return false;

        if (tier >= 6)
            return false;

        Merge(other);

        return true;
    }

    // =====================================================
    // MERGE
    // =====================================================

    private void Merge(Box other)
    {
        isMerging = true;
        other.isMerging = true;

        Vector2 mergePosition =
            (
                (Vector2)transform.position +
                (Vector2)other.transform.position
            ) * 0.5f;

        // ================================================
        // 1. ผลักของรอบตัวก่อน
        // ================================================

        ApplyExplosion(
            mergePosition,
            other
        );

        int nextTier = tier + 1;

        string nextVariant = variant;

        if (database == null)
            database =
                FindAnyObjectByType<BoxDatabase>();

        if (database == null)
        {
            Debug.LogError(
                "Box: BoxDatabase not found."
            );

            return;
        }

        // ================================================
        // 2. หา Prefab ตัวใหม่
        // ================================================

        GameObject nextPrefab =
            database.GetPrefab(
                nextTier,
                nextVariant
            );

        if (nextPrefab == null)
        {
            Debug.LogError(
                $"Box: Missing prefab for Tier {nextTier}, Variant {nextVariant}"
            );

            isMerging = false;
            other.isMerging = false;

            return;
        }

        // ================================================
        // 3. สร้างกล่องใหม่
        // ================================================

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
            newBox.SetDatabase(database);

            newBox.SetBoxData(
                nextTier,
                nextVariant
            );
        }

        // ================================================
        // 4. ให้กล่องใหม่ไม่โดนแรงส่งมั่ว
        // ================================================

        Rigidbody2D newRb =
            newBoxObject.GetComponent<Rigidbody2D>();

        if (newRb != null)
        {
            newRb.simulated = true;

            newRb.linearVelocity = Vector2.zero;
            newRb.angularVelocity = 0f;

            // ลดแรงเสียดทานจากการเกิดในกอง
            newRb.Sleep();
        }

        // ================================================
        // 5. ลบกล่องเก่า
        // ================================================

        Destroy(gameObject);
        Destroy(other.gameObject);

        // ================================================
        // 6. Score
        // ================================================

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(tier);
        }
    }

    // =====================================================
    // EXPLOSION
    // =====================================================

    private void ApplyExplosion(
        Vector2 explosionPosition,
        Box otherMergedBox
    )
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                explosionPosition,
                explosionRadius
            );

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            Rigidbody2D targetRb =
                hit.attachedRigidbody;

            if (targetRb == null)
                continue;

            // ไม่ผลักตัวเอง
            if (targetRb.gameObject == gameObject)
                continue;

            // ไม่ผลักอีกกล่องที่กำลัง Merge
            if (targetRb.gameObject ==
                otherMergedBox.gameObject)
                continue;

            Box targetBox =
                targetRb.GetComponent<Box>();

            if (targetBox != null &&
                targetBox.isMerging)
                continue;

            Vector2 offset =
                targetRb.worldCenterOfMass -
                explosionPosition;

            float distance =
                offset.magnitude;

            if (distance < 0.05f)
            {
                offset =
                    Random.insideUnitCircle.normalized;

                distance = 0.05f;
            }

            Vector2 direction =
                offset.normalized;

            // ยิ่งใกล้ ยิ่งแรง
            float falloff =
                1f -
                Mathf.Clamp01(
                    distance /
                    explosionRadius
                );

            float finalForce =
                explosionForce *
                falloff;

            Vector2 force =
                direction *
                finalForce;

            // ดันขึ้นเล็กน้อย
            force.y +=
                explosionUpForce *
                falloff;

            // เคลียร์แรงเดิมก่อน
            targetRb.linearVelocity *= 0.25f;

            targetRb.AddForce(
                force,
                ForceMode2D.Impulse
            );
        }
    }

    public void Explode(
    float radius,
    float force,
    float upForce)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                radius
            );

        Vector2 explosionPosition =
            transform.position;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (hit.gameObject == gameObject)
                continue;

            Rigidbody2D otherRb =
                hit.GetComponent<Rigidbody2D>();

            if (otherRb == null)
                continue;

            Vector2 direction =
                (Vector2)otherRb.transform.position
                - explosionPosition;

            float distance =
                direction.magnitude;

            if (distance < 0.01f)
                direction = Vector2.up;
            else
                direction.Normalize();

            float falloff =
                1f - Mathf.Clamp01(distance / radius);

            Vector2 explosion =
                direction *
                (force * falloff);

            explosion.y +=
                upForce * falloff;

            otherRb.WakeUp();

            otherRb.AddForce(
                explosion,
                ForceMode2D.Impulse
            );
        }

        Destroy(gameObject);
    }

    // =====================================================
    // Debug
    // =====================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}