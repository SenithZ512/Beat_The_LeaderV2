using UnityEngine;

public class ThrowWeaponPhysics : MonoBehaviour
{
    Rigidbody2D rb;

    public float spin = 600f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.angularVelocity = spin; // หมุนตอนบิน
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Leader"))
        {
            rb.angularVelocity = 0; // หยุดหมุน
            rb.velocity *= 0.3f;   // ชะลอความเร็ว
        }
    }
}