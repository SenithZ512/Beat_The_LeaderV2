using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 2;
    public float knockbackForce = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Leader"))
        {
            LeaderHealth leader = other.GetComponent<LeaderHealth>();

            if (leader != null)
            {
                leader.TakeDamage(damage);
            }

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 dir = (other.transform.position - transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}