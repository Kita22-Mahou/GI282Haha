using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSpawner : MonoBehaviour
{
    [Header("References")]
    public BoxDatabase database;
    public BoxQueue boxQueue;
    public NextBoxUI nextBoxUI;

    [Header("Spawn Area")]
    public float minX = -2.5f;
    public float maxX = 2.5f;
    public float spawnY = 5.5f;

    [Header("Movement")]
    public float moveSpeed = 2.5f;

    [Header("Timing")]
    [Min(0f)] public float nextSpawnDelay = 0.4f;

    private GameObject currentBox;
    private bool canMove;
    private bool spawnScheduled;

    private void Start()
    {
        if (database == null || boxQueue == null)
        {
            Debug.LogError("BoxSpawner: Assign Database and Box Queue in the Inspector.");
            enabled = false;
            return;
        }

        SpawnBox();
    }

    private void Update()
    {
        if (currentBox == null)
            return;

        if (canMove)
            MoveBoxAutomatically();

        CheckDropInput();
    }

    private void CheckDropInput()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DropBox();
        }
    }

    private void MoveBoxAutomatically()
    {
        Vector3 position = currentBox.transform.position;

        position.x += moveDirection * moveSpeed * Time.deltaTime;

        if (position.x >= maxX)
        {
            position.x = maxX;
            moveDirection = -1f;
        }
        else if (position.x <= minX)
        {
            position.x = minX;
            moveDirection = 1f;
        }

        position.y = spawnY;
        currentBox.transform.position = position;
    }

    private float moveDirection = 1f;

    private void SpawnBox()
    {
        spawnScheduled = false;

        int level = boxQueue.ConsumeNextLevel();
        GameObject prefab = database.GetPrefab(level);

        if (prefab == null)
        {
            Debug.LogError($"BoxSpawner: No prefab assigned for Level {level}.");
            return;
        }

        currentBox = Instantiate(
            prefab,
            new Vector3(0f, spawnY, 0f),
            Quaternion.identity
        );

        Box box = currentBox.GetComponent<Box>();
        if (box != null)
            box.SetLevel(level);

        Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        moveDirection = 1f;
        canMove = true;

        if (nextBoxUI != null)
            nextBoxUI.Refresh(boxQueue, database);
    }

    public void DropBox()
    {
        if (currentBox == null || !canMove)
            return;

        canMove = false;

        Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }

        currentBox = null;

        if (!spawnScheduled)
        {
            spawnScheduled = true;
            Invoke(nameof(SpawnBox), nextSpawnDelay);
        }
    }

    public int GetCurrentLevel()
    {
        if (currentBox == null)
            return 0;

        Box box = currentBox.GetComponent<Box>();
        return box != null ? box.Level : 0;
    }
}
