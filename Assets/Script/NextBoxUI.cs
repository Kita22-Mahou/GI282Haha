using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBoxUI : MonoBehaviour
{
    [Header("NEXT")]
    [SerializeField]
    private Image nextImage;

    [Header("UPCOMING")]
    [SerializeField]
    private Image[] upcomingImages;

    public void Refresh(
        BoxQueue queue,
        BoxDatabase database
    )
    {
        if (queue == null ||
            database == null)
            return;

        // =====================================
        // NEXT
        // =====================================

        SetImage(
            nextImage,
            queue.GetNextIndex(),
            database
        );

        // =====================================
        // UPCOMING
        // =====================================

        List<int> upcoming =
            queue.GetUpcomingIndices();

        // ปิดทั้งหมดก่อน
        for (int i = 0;
             i < upcomingImages.Length;
             i++)
        {
            if (upcomingImages[i] == null)
                continue;

            upcomingImages[i].enabled = false;
            upcomingImages[i].sprite = null;
        }

        // ใส่ Queue จริง
        for (
            int i = 0;
            i < upcoming.Count &&
            i < upcomingImages.Length;
            i++
        )
        {
            SetImage(
                upcomingImages[i],
                upcoming[i],
                database
            );
        }
    }

    private void SetImage(
        Image image,
        int index,
        BoxDatabase database
    )
    {
        if (image == null)
            return;

        image.enabled = false;
        image.sprite = null;

        if (index < 0)
            return;

        GameObject prefab =
            database.GetPrefabByIndex(index);

        if (prefab == null)
            return;

        SpriteRenderer sr =
            prefab.GetComponent<SpriteRenderer>();

        if (sr == null ||
            sr.sprite == null)
            return;

        image.sprite = sr.sprite;
        image.color = sr.color;
        image.preserveAspect = true;
        image.enabled = true;
    }
}