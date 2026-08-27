using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSpawner : MonoBehaviour
{
    [Header("Box")]
    public GameObject boxPrefab;

    [Header("Spawn Area")]
    public float minX = -2.5f;
    public float maxX = 2.5f;
    public float spawnY = 5.5f;

    [Header("Movement")]
    public float moveSpeed = 2.5f;

    private GameObject currentBox;

    // 1 = ไปขวา
    // -1 = ไปซ้าย
    private float moveDirection = 1f;

    private bool canMove = true;

    void Start()
    {
        SpawnBox();
    }

    void Update()
    {
        if (currentBox == null)
            return;

        MoveBoxAutomatically();
        CheckDrop();
    }

    // ========================================
    // Spawn Box
    // ========================================

    void SpawnBox()
    {
        Vector3 spawnPosition =
            new Vector3(0, spawnY, 0);

        currentBox = Instantiate(
            boxPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Rigidbody2D rb =
            currentBox.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // ยังไม่ให้ตก
            rb.simulated = false;
        }

        moveDirection = 1f;
        canMove = true;
    }

    // ========================================
    // Box เคลื่อนซ้าย-ขวาเอง
    // ========================================

    void MoveBoxAutomatically()
    {
        if (!canMove)
            return;

        Vector3 position =
            currentBox.transform.position;

        position.x +=
            moveDirection *
            moveSpeed *
            Time.deltaTime;

        // ชนขอบขวา
        if (position.x >= maxX)
        {
            position.x = maxX;

            moveDirection = -1f;
        }

        // ชนขอบซ้าย
        if (position.x <= minX)
        {
            position.x = minX;

            moveDirection = 1f;
        }

        position.y = spawnY;

        currentBox.transform.position =
            position;
    }

    // ========================================
    // Spacebar = Drop
    // ========================================

    void CheckDrop()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DropBox();
        }
    }

    // ========================================
    // Drop Box
    // ========================================

    void DropBox()
    {
        if (currentBox == null)
            return;

        // หยุดการเคลื่อนที่
        canMove = false;

        Rigidbody2D rb =
            currentBox.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // เปิด Physics
            rb.simulated = true;
        }

        currentBox = null;

        // รอก่อนสร้างกล่องใหม่
        Invoke(
            nameof(SpawnBox),
            0.1f
        );
    }
}