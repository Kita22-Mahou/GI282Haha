using UnityEngine;

public class BoxDatabase : MonoBehaviour
{
    [Header("Level Prefabs")]
    [Tooltip("Element 0 = Level 1, Element 7 = Level 8")]
    public GameObject[] boxPrefabs = new GameObject[8];

    public GameObject GetPrefab(int level)
    {
        if (level < 1 || level > boxPrefabs.Length)
        {
            Debug.LogWarning($"BoxDatabase: Invalid level {level}.");
            return null;
        }

        return boxPrefabs[level - 1];
    }

    public bool HasLevel(int level)
    {
        return level >= 1 &&
               level <= boxPrefabs.Length &&
               boxPrefabs[level - 1] != null;
    }
}
