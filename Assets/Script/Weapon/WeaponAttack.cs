using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour
{
    bool attacking = false;
    public float force = 500f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!attacking)
        {
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        attacking = true;

        transform.Rotate(0, 0, -80);
        yield return new WaitForSeconds(0.1f);

        transform.Rotate(0, 0, 80);

        attacking = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!attacking) return;

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