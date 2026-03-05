using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;

    GameObject currentWeapon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            currentWeapon = Instantiate(weaponPrefab, mousePos, Quaternion.identity);
        }

        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            currentWeapon.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(currentWeapon);
        }
    }
}