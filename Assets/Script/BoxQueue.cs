using System.Collections.Generic;
using UnityEngine;

public class BoxQueue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxDatabase database;

    [Header("Random Tier")]
    [SerializeField] private int minTier = 1;
    [SerializeField] private int maxTier = 3;

    [Header("Upcoming")]
    [Min(1)]
    [SerializeField] private int upcomingCount = 3;

    // Queue:
    // [0] = Current
    // [1] = Next
    // [2+] = Upcoming
    private readonly Queue<int> queue =
        new Queue<int>();

    private void Awake()
    {
        if (database == null)
            database =
                FindAnyObjectByType<BoxDatabase>();

        if (database == null)
        {
            Debug.LogError(
                "BoxQueue: BoxDatabase not found."
            );

            return;
        }

        minTier = Mathf.Max(1, minTier);
        maxTier = Mathf.Max(minTier, maxTier);
        upcomingCount = Mathf.Max(1, upcomingCount);

        InitializeQueue();
    }

    // =========================================
    // เริ่มต้น Queue
    // =========================================

    private void InitializeQueue()
    {
        queue.Clear();

        // Current 1
        queue.Enqueue(GetRandomIndex());

        // Next 1
        queue.Enqueue(GetRandomIndex());

        // Upcoming
        for (int i = 0;
             i < upcomingCount;
             i++)
        {
            queue.Enqueue(GetRandomIndex());
        }

        DebugQueue();
    }

    // =========================================
    // CURRENT
    // =========================================

    public int GetCurrentIndex()
    {
        if (queue.Count == 0)
            return -1;

        return queue.Peek();
    }

    // =========================================
    // NEXT 1 ตัว
    // =========================================

    public int GetNextIndex()
    {
        if (queue.Count < 2)
            return -1;

        int count = 0;

        foreach (int index in queue)
        {
            if (count == 1)
                return index;

            count++;
        }

        return -1;
    }

    // =========================================
    // UPCOMING
    // =========================================

    public List<int> GetUpcomingIndices()
    {
        List<int> result =
            new List<int>();

        int count = 0;

        foreach (int index in queue)
        {
            // ข้าม Current
            if (count == 0)
            {
                count++;
                continue;
            }

            // ข้าม Next
            if (count == 1)
            {
                count++;
                continue;
            }

            result.Add(index);

            count++;
        }

        return result;
    }

    // =========================================
    // เรียก "หลังจาก Current ตกแล้ว"
    // =========================================

    public void AdvanceQueue()
    {
        if (queue.Count == 0)
        {
            InitializeQueue();
            return;
        }

        // ลบ Current
        queue.Dequeue();

        // เติมตัวใหม่ท้าย Queue
        queue.Enqueue(
            GetRandomIndex()
        );

        DebugQueue();
    }

    // =========================================
    // RANDOM เฉพาะ Tier 1-3
    // =========================================

    private int GetRandomIndex()
    {
        List<int> validIndices =
            new List<int>();

        for (int i = 0;
             i < database.boxData.Length;
             i++)
        {
            BoxDatabase.BoxEntry entry =
                database.boxData[i];

            if (entry == null)
                continue;

            if (entry.prefab == null)
                continue;

            if (entry.tier < minTier ||
                entry.tier > maxTier)
                continue;

            validIndices.Add(i);
        }

        if (validIndices.Count == 0)
        {
            Debug.LogError(
                "BoxQueue: No valid prefab found."
            );

            return -1;
        }

        return validIndices[
            Random.Range(
                0,
                validIndices.Count
            )
        ];
    }

    // =========================================
    // DEBUG
    // =========================================

    private void DebugQueue()
    {
        string message =
            "QUEUE: ";

        int i = 0;

        foreach (int index in queue)
        {
            string label =
                GetLabel(index);

            if (i == 0)
                message +=
                    $"[CURRENT {label}] ";

            else if (i == 1)
                message +=
                    $"[NEXT {label}] ";

            else
                message +=
                    $"[UPCOMING {label}] ";

            i++;
        }

        //Debug.Log(message);
    }

    private string GetLabel(int index)
    {
        if (index < 0 ||
            index >= database.boxData.Length)
        {
            return "NONE";
        }

        BoxDatabase.BoxEntry entry =
            database.boxData[index];

        if (entry == null)
            return "NONE";

        return
            $"T{entry.tier}_{entry.variant}";
    }
}