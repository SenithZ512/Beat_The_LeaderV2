using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage = 2;
    public GameObject currentWeaponPrefab;

    GameObject weaponInstance;   // เพิ่มตัวนี้

    public void SetWeapon(GameObject weapon)
    {
        if (weaponInstance != null)
        {
            Destroy(weaponInstance);
        }

        currentWeaponPrefab = weapon;
        weaponInstance = Instantiate(currentWeaponPrefab);
        weaponInstance.SetActive(false);
    }

    void Update()
    {
        if (weaponInstance == null) return;

        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;

        Vector3 pos = Camera.main.ScreenToWorldPoint(mouse);
        weaponInstance.transform.position = pos;

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