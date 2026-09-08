using UnityEngine;

public class Bigger : MonoBehaviour
{
    [Header("Growth")]
    [SerializeField] private float biggerTime = 2f;

    [Header("Detection")]
    [SerializeField] private LayerMask itemLayer;

    [Header("Jar Area")]
    [SerializeField]
    private Vector2 detectionSize =
        new Vector2(5f, 8f);

    [Header("Tier 6 Explosion")]
    [SerializeField] private float tier6MaxScale = 1.3f;
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 8f;
    [SerializeField] private float explosionUpForce = 2f;

    [SerializeField] private float maxScale;
    [SerializeField] private float biggerNum;

    private void Start()
    {

        InvokeRepeating(
            nameof(GrowBoxes),
            biggerTime,
            biggerTime
        );
    }

    private void GrowBoxes()
    {
        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                transform.position,
                detectionSize,
                0f,
                itemLayer
            );

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            Box box =
                hit.GetComponent<Box>();

            if (box == null)
                continue;

            float biggerAmount = GetGrowthAmount(box.Tier);

            Vector3 scale =
                box.transform.localScale;

            float newScale =
                Mathf.Min(
                    scale.x + biggerAmount,
                    GetMaxScale(box.Tier)
                );

            box.transform.localScale =
                new Vector3(
                    newScale,
                    newScale,
                    scale.z
                );

            if (box.Tier == 6 &&
    newScale >= tier6MaxScale)
            {
                box.Explode(
                    explosionRadius,
                    explosionForce,
                    explosionUpForce
                );
                GameManager.Instance.AddScore(25);
            }

        }
    }

    private float GetMaxScale(int tier)
    {
        switch (tier)
        {
            case 1:
                return maxScale;

            case 2:
                return maxScale;

            case 3:
                return maxScale;

            case 4:
                return maxScale;

            case 5:
                return maxScale;

            case 6:
                return tier6MaxScale;

            default:
                return maxScale;
        }
    }
    private float GetGrowthAmount(int tier)
    {
        switch (tier)
        {
            case 1:
                return biggerNum;

            case 2:
                return biggerNum;

            case 3:
                return biggerNum;

            case 4:
                return biggerNum;

            case 5:
                return biggerNum;

            case 6:
                return 0.1f;

            default:
                return biggerNum;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(
            transform.position,
            detectionSize
        );
    }
}