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

    public AudioClip hurtSound;
    private AudioSource audioSource;

    public int rewardMoney = 10;
    public PlayerMoney playerMoney;

    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        health = maxHealth;

        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            sr.sprite = normalSprite;

        audioSource = GetComponent<AudioSource>();

        // ถ้าไม่มี AudioSource ให้สร้างใหม่
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        // เล่นเสียงแบบ OneShot
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0)
        {
            Die();
        }
        else if (health <= maxHealth * 0.3f)
        {
            sr.sprite = criticalSprite;
        }
        else if (health <= maxHealth * 0.6f)
        {
            sr.sprite = hurtSprite;
        }
        else
        {
            sr.sprite = normalSprite;
        }
    }

    void Die()
    {
        isDead = true;

        sr.sprite = deadSprite;

        if (playerMoney != null)
        {
            playerMoney.money += rewardMoney;
        }

        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        health = maxHealth;
        isDead = false;

        sr.sprite = normalSprite;
    }
}