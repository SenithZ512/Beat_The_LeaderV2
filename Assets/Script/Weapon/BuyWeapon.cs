using UnityEngine;
using TMPro;

public class BuyWeapon : MonoBehaviour
{
    public PlayerMoney playerMoney;
    public Attack attack;

    public GameObject weaponPrefab;
    public int price;

    public TextMeshProUGUI buttonText;

    bool purchased = false;

    public void Buy()
    {
        if (!purchased)
        {
            if (MoneyManager.Instance.SpendMoney(price))
            {
                purchased = true;

                buttonText.text = "Equip";
            }
        }

        attack.SetWeapon(weaponPrefab);

        Debug.Log("Equip weapon");
    }
}