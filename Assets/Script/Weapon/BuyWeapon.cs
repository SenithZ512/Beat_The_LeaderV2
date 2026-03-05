using UnityEngine;
using TMPro;

public class BuyWeapon : MonoBehaviour
{
    public PlayerMoney playerMoney;
    public Attack attack;

    public GameObject weaponPrefab;
    public int price;

    public TextMeshProUGUI buttonText; // ใช้ Text เดิมของปุ่ม

    bool purchased = false;

    public void Buy()
    {
        if (!purchased)
        {
            if (playerMoney.money >= price)
            {
                playerMoney.money -= price;
                purchased = true;

                buttonText.text = "Equip"; // เปลี่ยนข้อความ
            }
        }

        attack.SetWeapon(weaponPrefab);

        Debug.Log("Equip weapon");
    }
}