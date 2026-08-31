using UnityEditor;
using UnityEngine;

public class OverlabPush : MonoBehaviour
{
    public float force = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Transform thisRoot = transform.root;
        //Transform otherRoot = collision.transform.root;
        //if (thisRoot == otherRoot)
        //{
        //    return;
        //}

        GameObject other = collision.gameObject;
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 direction = (other.transform.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
