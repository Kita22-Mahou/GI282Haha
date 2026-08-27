using UnityEngine;

[CreateAssetMenu(
    fileName = "BoxConfig",
    menuName = "Box Merge/Box Config"
)]
public class BoxConfig : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int level;

        public Vector2 size;

        public float mass;

        public int score;

        public Sprite sprite;

        public Color color = Color.white;
    }

    public LevelData[] levels;

    public LevelData GetLevel(int level)
    {
        if (level < 1 || level > levels.Length)
            return null;

        return levels[level - 1];
    }
}