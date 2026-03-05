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
        public int price;              // 7000
        public float addIncomePerSec;  // +4
        public float speedMultiplier;  // x1.7
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
    }

    string Key(string id) => "UPG_" + id;

    public bool IsBought(string id) => PlayerPrefs.GetInt(Key(id), 0) == 1;

    public bool TryBuy(string id)
    {
        var def = Get(id);
        if (def == null) return false;

        if (IsBought(id)) return false;

        // ใช้เงินจากกระเป๋า (currentMoney)
        if (!MoneyManager.Instance.SpendMoney(def.price)) return false;

        // เซฟว่าซื้อแล้ว
        PlayerPrefs.SetInt(Key(id), 1);
        PlayerPrefs.Save();

        // Apply ผลของอัปเกรดทันที
        ApplyOne(def);

        // รีเฟรช UI ปุ่ม (Owned/Buy)
        RefreshAllUpgradeButtons();

        return true;
    }

    public void ApplyPurchasedUpgrades()
    {
        // เริ่มจากค่า base ก่อน แล้วค่อยบวกคืนจากที่ซื้อ
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        foreach (var def in upgrades)
        {
            if (IsBought(def.id))
                ApplyOne(def);
        }

        RefreshAllUpgradeButtons();
    }

    void ApplyOne(UpgradeDef def)
    {
        MoneyManager.Instance.incomePerSecond += def.addIncomePerSec;

        // คูณสปีด (เช่น 1f * 1.7f * 2.2f ...)
        MoneyManager.Instance.speedMultiplier *= Mathf.Max(1f, def.speedMultiplier);
    }

    UpgradeDef Get(string id)
    {
        foreach (var u in upgrades)
            if (u.id == id) return u;
        return null;
    }

    // ✅ รีเซ็ตเฉพาะอัปเกรด (เงินไม่หาย ซื้อใหม่ได้ทันที)
    public void ResetUpgrades()
    {
        foreach (var def in upgrades)
            PlayerPrefs.DeleteKey(Key(def.id));

        PlayerPrefs.Save();

        // คืนค่า base ทันที
        MoneyManager.Instance.incomePerSecond = baseIncomePerSecond;
        MoneyManager.Instance.speedMultiplier = baseSpeedMultiplier;

        // รีเฟรชปุ่มทั้งหมดให้กลับมากดได้
        RefreshAllUpgradeButtons();
    }

    void RefreshAllUpgradeButtons()
    {
        // ต้องมีสคริปต์ UpgradeBuyButton อยู่บนปุ่ม
        var buttons = FindObjectsOfType<Upgrade>(true);
        foreach (var b in buttons)
        {
            b.RefreshUI(); // ต้องเป็น public ใน UpgradeBuyButton
        }
    }
}