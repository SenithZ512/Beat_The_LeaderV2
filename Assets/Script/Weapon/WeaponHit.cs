using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public float force = 500f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit something : " + col.name);
        if (col.CompareTag("Leader"))
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;
                rb.AddForce(dir * force);
            }
        }
    }
}