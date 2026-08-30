using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxDatabase database;
    [SerializeField] private BoxQueue boxQueue;
    [SerializeField] private NextBoxUI nextBoxUI;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;
    [SerializeField] private float spawnY = 5.5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Timing")]
    [SerializeField] private float nextSpawnDelay = 0.4f;

    private GameObject currentBox;
    private bool canMove;
    private float moveDirection = 1f;

    private void Start()
    {
        if (database == null)
            database =
                FindFirstObjectByType<BoxDatabase>();

        if (boxQueue == null)
            boxQueue =
                FindFirstObjectByType<BoxQueue>();

        if (database == null || boxQueue == null)
        {
            Debug.LogError(
                "BoxSpawner: Database or BoxQueue missing."
            );

            enabled = false;
            return;
        }

        SpawnCurrentBox();
    }

    private void Update()
    {
        if (currentBox == null)
            return;

        if (canMove)
            MoveBox();

        CheckDrop();
    }

    private void MoveBox()
    {
        Vector3 pos =
            currentBox.transform.position;

        pos.x +=
            moveDirection *
            moveSpeed *
            Time.deltaTime;

        if (pos.x >= maxX)
        {
            pos.x = maxX;
            moveDirection = -1f;
        }
        else if (pos.x <= minX)
        {
            pos.x = minX;
            moveDirection = 1f;
        }

        pos.y = spawnY;

        currentBox.transform.position =
            pos;
    }

    private void CheckDrop()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DropBox();
        }
    }

    // =========================================
    // Spawn CURRENT
    // =========================================

    private void SpawnCurrentBox()
    {
        int index =
            boxQueue.GetCurrentIndex();

        if (index < 0)
            return;

        GameObject prefab =
            database.GetPrefabByIndex(index);

        if (prefab == null)
        {
            Debug.LogError(
                $"BoxSpawner: Prefab index {index} is missing."
            );

            return;
        }

        currentBox =
            Instantiate(
                prefab,
                new Vector3(
                    0f,
                    spawnY,
                    0f
                ),
                Quaternion.identity
            );

        Box box =
            currentBox.GetComponent<Box>();

        if (box != null)
{
    box.SetDatabase(database);
}

        Rigidbody2D rb =
            currentBox.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        moveDirection = 1f;
        canMove = true;

        RefreshNextUI();
    }

    // =========================================
    // SPACE = DROP
    // =========================================

    public void DropBox()
    {
        if (currentBox == null ||
            !canMove)
            return;

        canMove = false;

        Rigidbody2D rb =
            currentBox.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = true;
        }

        currentBox = null;

        // สำคัญ:
        // ยังไม่เลื่อน Queue ตอนนี้
        // รอให้ Spawn ตัวต่อไป
        Invoke(
            nameof(SpawnNextBox),
            nextSpawnDelay
        );
    }

    private void SpawnNextBox()
    {
        // Current เก่าจบแล้ว
        // ตอนนี้ค่อยเลื่อน Queue
        boxQueue.AdvanceQueue();

        SpawnCurrentBox();
    }

    private void RefreshNextUI()
    {
        if (nextBoxUI != null)
        {
            nextBoxUI.Refresh(
                boxQueue,
                database
            );
        }
    }
}