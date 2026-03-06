using UnityEngine;

public class HitLeader : MonoBehaviour
{
    public int damage = 5;
    public float knockbackForce = 5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Leader"))
        {
            LeaderHealth leader = collision.gameObject.GetComponent<LeaderHealth>();

            if (leader != null)
            {
                leader.TakeDamage(damage);
            }

            // ทำให้อาวุธกระเด็น
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 bounce = -collision.contacts[0].normal;
                rb.AddForce(bounce * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}