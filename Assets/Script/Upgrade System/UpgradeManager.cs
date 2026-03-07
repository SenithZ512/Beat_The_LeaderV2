using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [System.Serializable]
    public class UpgradeDef
    {
        public string id;              // เช่น GPU_2060 (ห้ามซ้ำ)
        public int price;              // ราคา เช่น 7000
        public float addIncomePerSec;  // เพิ่มเงินต่อวินาที
        public float speedMultiplier;  // ตัวคูณสปีด
    }

    [Header("List of Upgrades")]
    public UpgradeDef[] upgrades;

    [Header("Base Values (ตอนยังไม่อัปเกรด)")]
    public float baseIncomePerSecond = 1f;
    public float baseSpeedMultiplier = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // เพิ่มบรรทัดนี้เพื่อไม่ให้หายตอนเปลี่ยน Scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ApplyPurchasedUpgrades();
    }

    string Key(string id)
    {
        return "UPG_" + id;
    }

    public bool IsBought(string id)
    {
        return PlayerPrefs.GetInt(Key(id), 0) == 1;
    }

    public bool TryBuy(string id)
    {
        var def = Get(id);
        if (def == null) return false;

        if (IsBought(id)) return false;

        // ใช้เงิน
        if (!MoneyManager.Instance.SpendMoney(def.price))
            return false;

        // บันทึกว่าซื้อแล้ว
        PlayerPrefs.SetInt(Key(id), 1);
        PlayerPrefs.Save();

        // ใช้เอฟเฟกต์อัปเกรด
        ApplyOne(def);

        // รีเฟรช UI
        RefreshAllUpgradeButtons();

        return true;
    }

    public void ApplyPurchasedUpgrades()
    {
        // เริ่มจากค่า base ก่อน
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        foreach (var def in upgrades)
        {
            if (IsBought(def.id))
            {
                ApplyOne(def);
            }
        }

        RefreshAllUpgradeButtons();
    }

    void ApplyOne(UpgradeDef def)
    {
        MoneyManager.Instance.incomePerSecond += def.addIncomePerSec;

        // คูณสปีด
        MoneyManager.Instance.speedMultiplier *= Mathf.Max(1f, def.speedMultiplier);
    }

    UpgradeDef Get(string id)
    {
        foreach (var u in upgrades)
        {
            if (u.id == id)
                return u;
        }

        return null;
    }

    // รีเซ็ตอัปเกรด
    public void ResetUpgrades()
    {
        foreach (var def in upgrades)
        {
            PlayerPrefs.DeleteKey(Key(def.id));
        }

        PlayerPrefs.Save();

        // คืนค่า base
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        // รีเฟรชปุ่ม
        RefreshAllUpgradeButtons();
    }

    void RefreshAllUpgradeButtons()
    {
        var buttons = FindObjectsOfType<Upgrade>(true);

        foreach (var b in buttons)
        {
            b.RefreshUI();
        }
    }
}