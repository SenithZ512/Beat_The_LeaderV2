using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("Wallet Money (ใช้ซื้อของได้)")]
    public int currentMoney = 0;
    public TextMeshProUGUI moneyText;

    [Header("Unclaimed Money (สะสมรอรับ)")]
    public float incomePerSecond = 1f;
    public float unclaimedMoney = 0f;
    public TextMeshProUGUI unclaimedText;

    [Header("Popup ตอนรับเงิน (optional)")]
    public TextMeshProUGUI gainedPopupText;

    const string KEY_MONEY = "Money";
    const string KEY_UNCLAIMED = "UnclaimedMoney";

    float saveTimer = 0f;
    const float SAVE_INTERVAL = 1f; // เซฟทุก 1 วินาทีพอ


    [Header("Speed Multiplier")]
    public float speedMultiplier = 1f;
    private void Awake()
    {
        // กัน MoneyManager ซ้ำ (สำคัญมากถ้าเปลี่ยนซีน/มี prefab ซ้ำ)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        LoadData();
        UpdateUI();
        UpdateUnclaimedUI();
        if (gainedPopupText) gainedPopupText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // ✅ Passive สะสมเข้ากองรอรับ
        unclaimedMoney += incomePerSecond * speedMultiplier * Time.deltaTime;
        UpdateUnclaimedUI();

        // ✅ Autosave แบบไม่ถี่เกินไป
        saveTimer += Time.deltaTime;
        if (saveTimer >= SAVE_INTERVAL)
        {
            saveTimer = 0f;
            SaveUnclaimedOnly();
        }
    }

    // ✅ คลิก/รางวัลต่างๆ: ให้ไปกองรอรับเหมือน passive (รวมกัน)
    public void AddMoney(int amount)
    {
        AddToUnclaimed(amount);
    }

    // ถ้าอยากเพิ่มเข้ากระเป๋าทันทีจริงๆ (บางกรณีพิเศษ) ใช้อันนี้
    public void AddToWallet(int amount)
    {
        currentMoney += amount;
        SaveMoney();
        UpdateUI();
    }

    // ✅ เพิ่มเข้ากองรอรับ (ใช้กับ click ได้)
    public void AddToUnclaimed(int amount)
    {
        unclaimedMoney += amount;
        UpdateUnclaimedUI();
        SaveUnclaimedOnly();
    }

    // ✅ ใช้เงินซื้อของ (ใช้ currentMoney เท่านั้น)
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            SaveMoney();
            UpdateUI();
            return true;
        }
        return false;
    }

    // ✅ กด ClaimAll แล้วรวม (click+passive) เข้า wallet ทีเดียว
    public void ClaimAll()
    {
        int claim = Mathf.FloorToInt(unclaimedMoney);
        if (claim <= 0) return;

        currentMoney += claim;
        unclaimedMoney -= claim; // หรือ unclaimedMoney = 0f;

        SaveData();
        UpdateUI();
        UpdateUnclaimedUI();

        ShowPopup(claim);
    }

    void UpdateUI()
    {
        if (moneyText) moneyText.text = currentMoney + " $";
    }

    void UpdateUnclaimedUI()
    {
        if (unclaimedText) unclaimedText.text = Mathf.FloorToInt(unclaimedMoney) + " $";
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt(KEY_MONEY, currentMoney);
        PlayerPrefs.Save();
    }

    void SaveUnclaimedOnly()
    {
        PlayerPrefs.SetFloat(KEY_UNCLAIMED, unclaimedMoney);
        PlayerPrefs.Save();
    }

    void SaveData()
    {
        PlayerPrefs.SetInt(KEY_MONEY, currentMoney);
        PlayerPrefs.SetFloat(KEY_UNCLAIMED, unclaimedMoney);
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        currentMoney = PlayerPrefs.GetInt(KEY_MONEY, 0);
        unclaimedMoney = PlayerPrefs.GetFloat(KEY_UNCLAIMED, 0f);
    }

    public void ResetMoney()
    {
        PlayerPrefs.DeleteKey(KEY_MONEY);
        PlayerPrefs.DeleteKey(KEY_UNCLAIMED);
        PlayerPrefs.Save();

        currentMoney = 0;
        unclaimedMoney = 0f;

        UpdateUI();
        UpdateUnclaimedUI();
        if (gainedPopupText) gainedPopupText.gameObject.SetActive(false);
    }

    void ShowPopup(int amount)
    {
        if (!gainedPopupText) return;
        StopAllCoroutines();
        StartCoroutine(PopupRoutine(amount));
    }

    IEnumerator PopupRoutine(int amount)
    {
        gainedPopupText.gameObject.SetActive(true);
        gainedPopupText.text = $"+{amount} $";
        yield return new WaitForSeconds(1.0f);
        gainedPopupText.gameObject.SetActive(false);
    }
    public void ResetUpgradesOnly()
    {
        // คืนค่าเริ่มต้น
        incomePerSecond = 1f;
        speedMultiplier = 1f; // ถ้ามี
                              // ลบคีย์อัปเกรดทั้งหมดที่เคยซื้อ
                              // ต้องใส่ให้ตรงกับ upgradeId ที่คุณใช้จริง
        string[] upgradeIds = { "GPU_2060", "GPU_3070", "GPU_4080", "GPU_5080Ti" };

        foreach (var id in upgradeIds)
            PlayerPrefs.DeleteKey("UPG_" + id);

        PlayerPrefs.Save();
        UpdateUnclaimedUI();
        // ถ้ามี UpgradeManager ให้รีโหลดระบบ
        if (FindObjectOfType<UpgradeManager>())
        {
            FindObjectOfType<UpgradeManager>().SendMessage("LoadUpgrades", SendMessageOptions.DontRequireReceiver);
        }
        Debug.Log("Upgrades Reset Complete");
    }
}