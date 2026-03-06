using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    public Sprite gunIdle;
    public Sprite gunShoot;

    private SpriteRenderer sr;
    private AudioSource audioSource;

    public GameObject shootButton;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        sr.sprite = gunIdle;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        sr.sprite = gunShoot;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = -firePoint.right * bulletSpeed;

        Invoke("BackToIdle", 0.1f);
    }

    void BackToIdle()
    {
        sr.sprite = gunIdle;
    }
}