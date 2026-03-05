using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public int money = 50000;
    public TextMeshProUGUI moneyText;

    void Update()
    {
        moneyText.text = money.ToString();
    }

    public void SpendMoney(int price)
    {
        money -= price;
    }
}