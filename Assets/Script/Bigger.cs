using UnityEngine;

public class Bigger : MonoBehaviour
{
    [Header("Growth")]
    [SerializeField] private float biggerTime = 2f;

    [Header("Detection")]
    [SerializeField] private LayerMask boxLayer;

    [Header("Jar Area")]
    [SerializeField]
    private Vector2 detectionSize =
        new Vector2(5f, 8f);

    [Header("Tier 6 Explosion")]
    [SerializeField] private float tier6MaxScale = 1.5f;
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 8f;
    [SerializeField] private float explosionUpForce = 2f;

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
                boxLayer
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
            }
        }
    }

    private float GetMaxScale(int tier)
    {
        switch (tier)
        {
            case 1:
                return 1.0f;

            case 2:
                return 1.1f;

            case 3:
                return 1.2f;

            case 4:
                return 1.3f;

            case 5:
                return 1.4f;

            case 6:
                return tier6MaxScale;

            default:
                return 1.2f;
        }
    }
    private float GetGrowthAmount(int tier)
    {
        switch (tier)
        {
            case 1:
                return 0.03f;

            case 2:
                return 0.04f;

            case 3:
                return 0.05f;

            case 4:
                return 0.06f;

            case 5:
                return 0.07f;

            case 6:
                return 0.08f;

            default:
                return 0.05f;
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