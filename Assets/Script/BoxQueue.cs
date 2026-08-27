using System.Collections.Generic;
using UnityEngine;

public class BoxQueue : MonoBehaviour
{
    [Header("Random Level")]
    [Min(1)] public int minLevel = 1;
    [Min(1)] public int maxLevel = 3;

    [Header("Preview")]
    [Min(1)] public int previewCount = 3;

    private readonly Queue<int> levelQueue = new Queue<int>();

    public IReadOnlyCollection<int> Levels => levelQueue;

    private void Awake()
    {
        maxLevel = Mathf.Max(minLevel, maxLevel);
        previewCount = Mathf.Max(1, previewCount);
        FillQueue();
    }

    private void FillQueue()
    {
        levelQueue.Clear();

        for (int i = 0; i < previewCount; i++)
        {
            levelQueue.Enqueue(GetRandomLevel());
        }
    }

    public int ConsumeNextLevel()
    {
        if (levelQueue.Count == 0)
        {
            FillQueue();
        }

        int nextLevel = levelQueue.Dequeue();
        levelQueue.Enqueue(GetRandomLevel());

        return nextLevel;
    }

    public List<int> GetPreviewLevels()
    {
        return new List<int>(levelQueue);
    }

    public int GetRandomLevel()
    {
        return Random.Range(minLevel, maxLevel + 1);
    }
}
