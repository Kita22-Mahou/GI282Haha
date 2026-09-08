using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;

    [Header("Destroy")]
    [SerializeField] private float destroyDelay = 0.1f;

    [Header("PopOnSpawn")]
    [SerializeField] private float spawnSize = 0f;
    [SerializeField] private float normalSize;
    [SerializeField] private float plusNumber;

    private int destroyedCount = 0;

    private bool exploded = false;

    [SerializeField] private ParticleSystem popFX1;
    [SerializeField] private ParticleSystem popFX2;
    [SerializeField] private ParticleSystem popFX3;

    private void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 1);
        StartCoroutine(ItemPopOnSpawn());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(popFX1, this.transform.position, Quaternion.identity); // spawn particle effect
        Instantiate(popFX2, this.transform.position, Quaternion.identity);
        Instantiate(popFX3, this.transform.position, Quaternion.identity);

        if (exploded)
            return;

        exploded = true;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                explosionRadius
            );


        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            Box box =
                hit.GetComponent<Box>();

            if (box == null)
                continue;

            destroyedCount++;

            Destroy(
                box.gameObject,
                destroyDelay
            );

            Instantiate(popFX1, box.transform.position, Quaternion.identity); // spawn particle effect
            Instantiate(popFX2, box.transform.position, Quaternion.identity);
            Instantiate(popFX3, box.transform.position, Quaternion.identity);
        }

        if (destroyedCount > 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(
                    destroyedCount
                );
                Debug.Log($"++{destroyedCount} ");
                destroyedCount = 0;
            }

        }


        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }

    IEnumerator ItemPopOnSpawn()
    {
        float sizeNumCheck = 0f;

        while (sizeNumCheck <= normalSize)
        {
            sizeNumCheck += plusNumber;
            transform.localScale += new Vector3(plusNumber, plusNumber, 0);
            yield return null;
        }
    }
}
