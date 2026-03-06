using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [System.Serializable]
    public class UpgradeDef
    {
        public string id;              // ID ของ Upgrade
        public int price;              // ราคา
        public float addIncomePerSec;  // เพิ่มเงินต่อวินาที
        public float speedMultiplier;  // ตัวคูณสปีด
    }

    [Header("List of Upgrades")]
    public UpgradeDef[] upgrades;

    [Header("Base Values")]
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
    }

    void Start()
    {
        // โหลด upgrade ที่เคยซื้อ
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

        if (!MoneyManager.Instance.SpendMoney(def.price))
            return false;

        // บันทึกว่าซื้อแล้ว
        PlayerPrefs.SetInt(Key(id), 1);
        PlayerPrefs.Save();

        // ใช้ผล upgrade
        ApplyOne(def);

        // รีเฟรช UI
        RefreshAllUpgradeButtons();
        RefreshAllOwnText();

        return true;
    }

    public void ApplyPurchasedUpgrades()
    {
        // reset ค่า
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        // ใส่ effect ของ upgrade ที่ซื้อ
        foreach (var def in upgrades)
        {
            if (IsBought(def.id))
            {
                ApplyOne(def);
            }
        }

        // รีเฟรช UI
        RefreshAllUpgradeButtons();
        RefreshAllOwnText();
    }

    void ApplyOne(UpgradeDef def)
    {
        MoneyManager.Instance.incomePerSecond += def.addIncomePerSec;
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

    // ปุ่ม Reset Upgrade
    public void ResetUpgrades()
    {
        foreach (var def in upgrades)
        {
            PlayerPrefs.DeleteKey(Key(def.id));
        }

        PlayerPrefs.Save();

        // รีเซ็ตค่า stat
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        // โหลดสถานะใหม่
        ApplyPurchasedUpgrades();

        // รีเฟรช UI
        RefreshAllUpgradeButtons();
        RefreshAllOwnText();
    }

    void RefreshAllUpgradeButtons()
    {
        var buttons = FindObjectsOfType<Upgrade>(true);

        foreach (var b in buttons)
        {
            b.RefreshUI();
        }
    }

    void RefreshAllOwnText()
    {
        var owns = FindObjectsOfType<UpgradeOwnText>(true);

        foreach (var o in owns)
        {
            o.Refresh();
        }
    }
}