using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public static PlayerWeapon instance;

    public string currentWeapon;
    public int currentDamage;

    void Awake()
    {
        instance = this;
    }

    public void SetWeapon(string weaponName, int damage)
    {
        currentWeapon = weaponName;
        currentDamage = damage;

        Debug.Log("Weapon Changed To: " + weaponName);
    }
}