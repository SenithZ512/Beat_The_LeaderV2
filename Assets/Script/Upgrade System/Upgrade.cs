using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("Upgrade Setting")]
    public string upgradeId ; // ต้องไม่ซ้ำกัน
    public int price = 7000;             // ราคาที่โชว์บนปุ่ม
    public float addIncomePerSec = 4f;   // เพิ่ม coin/sec เท่าไหร่
    public float speedMultiplier = 1.7f;

    [Header("UI (optional)")]
    public Button buyButton;
    public TextMeshProUGUI buttonText;   // Text บนปุ่ม Buy (ให้เปลี่ยนเป็น Owned)
    public TextMeshProUGUI priceText;    // ช่องราคา (ถ้ามี)



    private string Key => "UPG_" + upgradeId;

    void Start()
    {
        RefreshUI();
        if (buyButton == null) buyButton = GetComponent<Button>();
        if (buyButton != null) buyButton.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        // เคยซื้อแล้ว
        if (IsBought()) { RefreshUI(); return; }

        // เงินไม่พอ (ใช้เงินในกระเป๋า)
        if (!MoneyManager.Instance.SpendMoney(price))
        {
            Debug.Log("Money not enough!");
            return;
        }

        // เพิ่ม passive income
        MoneyManager.Instance.incomePerSecond += addIncomePerSec;
        MoneyManager.Instance.speedMultiplier *= speedMultiplier;

        // เซฟว่าเคยซื้อแล้ว
        PlayerPrefs.SetInt(Key, 1);
        PlayerPrefs.Save();

        RefreshUI();
        Debug.Log("Bought " + upgradeId);
    }

    bool IsBought()
    {
        return PlayerPrefs.GetInt(Key, 0) == 1;
    }

    public void RefreshUI()
    {
        bool owned = IsBought();

        if (priceText && owned)
            priceText.text = "Owned";

        if (buyButton == null) buyButton = GetComponent<Button>();
        if (buyButton) buyButton.interactable = !owned;
    }
   
}