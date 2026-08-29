using UnityEngine;

public class BoxDatabase : MonoBehaviour
{
    [System.Serializable]
    public class BoxEntry
    {
        [Header("Box Info")]
        [Min(1)]
        public int tier = 1;

        [Tooltip("Use A or B for the two box types in each Tier.")]
        public string variant = "A";

        [Header("Prefab")]
        public GameObject prefab;
    }

    [Header("All Box Prefabs")]
    [Tooltip("12 entries total: Tier 1-6, two variants per Tier.")]
    public BoxEntry[] boxData = new BoxEntry[12];

    // --------------------------------------------------
    // Get a prefab by Tier + Variant
    // Example: GetPrefab(1, "A")
    // --------------------------------------------------
    public GameObject GetPrefab(int tier, string variant)
    {
        if (string.IsNullOrWhiteSpace(variant))
        {
            Debug.LogWarning("BoxDatabase: Variant is empty.");
            return null;
        }

        string targetVariant = variant.Trim().ToUpperInvariant();

        foreach (BoxEntry entry in boxData)
        {
            if (entry == null || entry.prefab == null)
                continue;

            if (entry.tier == tier &&
                !string.IsNullOrWhiteSpace(entry.variant) &&
                entry.variant.Trim().ToUpperInvariant() == targetVariant)
            {
                return entry.prefab;
            }
        }

        Debug.LogWarning(
            $"BoxDatabase: Prefab not found for Tier {tier}, Variant {targetVariant}."
        );

        return null;
    }

    // --------------------------------------------------
    // Get a prefab by array index (0-11)
    // --------------------------------------------------
    public GameObject GetPrefabByIndex(int index)
    {
        if (index < 0 || index >= boxData.Length)
        {
            Debug.LogWarning($"BoxDatabase: Invalid index {index}.");
            return null;
        }

        if (boxData[index] == null)
        {
            Debug.LogWarning($"BoxDatabase: Entry {index} is empty.");
            return null;
        }

        return boxData[index].prefab;
    }

    // --------------------------------------------------
    // Get the full BoxEntry by Tier + Variant
    // --------------------------------------------------
    public BoxEntry GetEntry(int tier, string variant)
    {
        if (string.IsNullOrWhiteSpace(variant))
            return null;

        string targetVariant = variant.Trim().ToUpperInvariant();

        foreach (BoxEntry entry in boxData)
        {
            if (entry == null)
                continue;

            if (entry.tier == tier &&
                !string.IsNullOrWhiteSpace(entry.variant) &&
                entry.variant.Trim().ToUpperInvariant() == targetVariant)
            {
                return entry;
            }
        }

        return null;
    }

    // --------------------------------------------------
    // Check whether a Tier + Variant exists
    // --------------------------------------------------
    public bool HasPrefab(int tier, string variant)
    {
        return GetPrefab(tier, variant) != null;
    }

    // --------------------------------------------------
    // Get both variants in a Tier
    // Returns A first, then B when available.
    // --------------------------------------------------
    public GameObject[] GetPrefabsByTier(int tier)
    {
        GameObject[] result = new GameObject[2];
        int found = 0;

        foreach (BoxEntry entry in boxData)
        {
            if (entry == null || entry.prefab == null)
                continue;

            if (entry.tier != tier)
                continue;

            if (found >= result.Length)
                break;

            result[found] = entry.prefab;
            found++;
        }

        return result;
    }

    // --------------------------------------------------
    // Get number of valid prefabs
    // --------------------------------------------------
    public int GetValidCount()
    {
        int count = 0;

        foreach (BoxEntry entry in boxData)
        {
            if (entry != null && entry.prefab != null)
                count++;
        }

        return count;
    }
}
