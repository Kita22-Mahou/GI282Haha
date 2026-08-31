using UnityEngine;

public class ItemOverlabPush : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 6f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Hit");
        PushOverlabItem(collision.gameObject);
    }

    void PushOverlabItem(GameObject box)
    {
        Vector2 explodePos =
            (
                transform.position +
                box.transform.position
            ) * 0.5f;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                explodePos,
                explosionRadius
            );

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            Rigidbody2D targetRb =
                hit.attachedRigidbody;

            if (targetRb == null)
                continue;

            // ไม่ผลักตัวเอง
            if (targetRb.gameObject == gameObject)
                continue;

            // ไม่ผลักอีกกล่องที่กำลัง Merge
            if (targetRb.gameObject ==
                box.gameObject)
                continue;

            Vector2 offset =
                targetRb.worldCenterOfMass -
                explodePos;

            float distance =
                offset.magnitude;

            if (distance < 0.05f)
            {
                offset =
                    Random.insideUnitCircle.normalized;

                distance = 0.05f;
            }

            Vector2 direction =
                offset.normalized;

            // ยิ่งใกล้ ยิ่งแรง
            float falloff =
                1f -
                Mathf.Clamp01(
                    distance /
                    explosionRadius
                );

            float finalForce =
                explosionForce *
                falloff;

            Vector2 force =
                direction *
                finalForce;

            // ดันขึ้นเล็กน้อย
            force.y +=
                explosionForce *
                falloff;

            targetRb.AddForce(
                force,
                ForceMode2D.Impulse
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}
