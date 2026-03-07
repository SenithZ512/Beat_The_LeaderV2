using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("Upgrade Setting")]
    public string upgradeId; // ต้องไม่ซ้ำกัน
    public int price = 7000;
    public float addIncomePerSec = 4f;
    public float speedMultiplier = 1.7f;

    [Header("UI (optional)")]
    public Button buyButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI priceText;

    private string Key => "UPG_" + upgradeId;

    void Start()
    {
        // ให้หา Button ก่อน
        if (buyButton == null)
            buyButton = GetComponent<Button>();

        if (buyButton != null)
            buyButton.onClick.AddListener(Buy);

        // ❌ ลบบรรทัดนี้ออก
        // PlayerPrefs.DeleteAll();

        RefreshUI();
    }

    public void Buy()
    {
        if (IsBought())
        {
            RefreshUI();
            return;
        }

        if (!MoneyManager.Instance.SpendMoney(price))
        {
            Debug.Log("Money not enough!");
            return;
        }

        MoneyManager.Instance.incomePerSecond += addIncomePerSec;
        MoneyManager.Instance.speedMultiplier *= speedMultiplier;

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

        if (buyButton == null)
            buyButton = GetComponent<Button>();

        if (buyButton != null)
            buyButton.interactable = !owned;

        if (priceText != null)
        {
            if (owned)
                priceText.text = "Owned";
            else
                priceText.text = price.ToString();
        }

        if (buttonText != null)
        {
            buttonText.text = owned ? "Owned" : "Buy";
        }
    }
}