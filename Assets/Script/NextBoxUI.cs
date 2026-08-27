using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBoxUI : MonoBehaviour
{
    [Header("UI Slots")]
    [Tooltip("Put the Image components in order: next, second-next, third-next...")]
    [SerializeField] private Image[] previewImages;

    [Header("Layout")]
    [SerializeField] private float previewSize = 90f;

    public void Refresh(BoxQueue queue, BoxDatabase database)
    {
        if (queue == null || database == null)
            return;

        List<int> levels = queue.GetPreviewLevels();

        for (int i = 0; i < previewImages.Length; i++)
        {
            Image image = previewImages[i];

            if (image == null)
                continue;

            if (i >= levels.Count)
            {
                image.enabled = false;
                continue;
            }

            int level = levels[i];
            GameObject prefab = database.GetPrefab(level);

            if (prefab == null)
            {
                image.enabled = false;
                continue;
            }

            SpriteRenderer spriteRenderer =
                prefab.GetComponent<SpriteRenderer>();

            Box box = prefab.GetComponent<Box>();

            image.enabled = true;

            if (spriteRenderer != null)
            {
                image.sprite = spriteRenderer.sprite;
                image.color = spriteRenderer.color;
            }
            else
            {
                image.sprite = null;
                image.color = Color.white;
            }

            float scale = 1f;

            if (box != null)
            {
                // Preview size follows each prefab's world scale.
                scale = Mathf.Max(
                    prefab.transform.localScale.x,
                    prefab.transform.localScale.y
                );
            }

            RectTransform rect = image.rectTransform;
            rect.sizeDelta = Vector2.one * previewSize * scale;
        }
    }
}
