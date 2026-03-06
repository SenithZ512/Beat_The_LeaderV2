using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage = 2;
    public GameObject currentWeaponPrefab;

    GameObject weaponInstance;
    bool isThrowWeapon;

    public void SetWeapon(GameObject weapon)
    {
        if (weaponInstance != null)
        {
            Destroy(weaponInstance);
        }

        currentWeaponPrefab = weapon;

        WeaponType wt = weapon.GetComponent<WeaponType>();

        if (wt != null)
        {
            isThrowWeapon = wt.isThrowWeapon;
        }

        if (!isThrowWeapon)
        {
            weaponInstance = Instantiate(currentWeaponPrefab);
            weaponInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (currentWeaponPrefab == null) return;

        if (isThrowWeapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ThrowWeapon();
            }
        }
        else
        {
            if (weaponInstance == null) return;

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;

            weaponInstance.transform.position = mouse;

            if (Input.GetMouseButtonDown(0))
            {
                weaponInstance.SetActive(true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                weaponInstance.SetActive(false);
            }
        }
    }

    void ThrowWeapon()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
        worldPos.z = 0f;

        // ให้เกิดตรงตำแหน่งเมาส์
        GameObject weapon = Instantiate(currentWeaponPrefab, worldPos, Quaternion.identity);

        Vector2 dir = (worldPos - transform.position).normalized;

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = dir * 15f;
        }
    }
}