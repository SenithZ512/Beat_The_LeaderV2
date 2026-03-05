using UnityEngine;

public class LeaderHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int health;

    public float respawnTime = 5f;

    public Sprite normalSprite;
    public Sprite hurtSprite;
    public Sprite criticalSprite;
    public Sprite deadSprite;

    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else if (health <= maxHealth * 0.3f)
        {
            Debug.Log("Leader สาหัส");
            sr.sprite = criticalSprite;
        }
        else if (health <= maxHealth * 0.6f)
        {
            Debug.Log("Leader เจ็บ");
            sr.sprite = hurtSprite;
        }
        else
        {
            Debug.Log("Leader ปกติ");
            sr.sprite = normalSprite;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Leader ตาย");

        sr.sprite = deadSprite;

        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        health = maxHealth;
        isDead = false;

        sr.sprite = normalSprite;

        Debug.Log("Leader ฟื้นแล้ว");
    }
}