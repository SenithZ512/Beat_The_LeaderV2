using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 2;
    public float knockbackForce = 5f;

    public AudioClip hitSound;          // เพิ่มเสียง
    private AudioSource audioSource;    // เพิ่ม AudioSource

    bool hasHit = false; // เช็คว่าตีไปแล้วหรือยัง

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // ดึง AudioSource จากอาวุธ
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHit) return;

        if (other.gameObject.CompareTag("Leader"))
        {
            LeaderHealth leader = other.gameObject.GetComponent<LeaderHealth>();

            if (leader != null)
            {
                leader.TakeDamage(damage);
            }

            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 dir = (other.transform.position - transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }

            // เล่นเสียงที่อาวุธ
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            hasHit = true; // ทำดาเมจได้แค่ครั้งเดียว
        }
    }
}