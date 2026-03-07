using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    const string KEY_SPEED = "SpeedMultiplier";
    const string KEY_INCOME = "IncomePerSecond";

    float saveTimer = 0f;
    const float SAVE_INTERVAL = 1f;

    [Header("Speed Multiplier")]
    public float speedMultiplier = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        LoadData();

        UpdateUI();
        UpdateUnclaimedUI();

        if (gainedPopupText != null)
            gainedPopupText.gameObject.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // หา UI ใหม่ทุก Scene
        moneyText = GameObject.Find("Text Coin")?.GetComponent<TextMeshProUGUI>();

        var passive = GameObject.Find("Passive Money");
        if (passive != null)
            unclaimedText = passive.GetComponentInChildren<TextMeshProUGUI>();

        UpdateUI();
        UpdateUnclaimedUI();
    }

    void Update()
    {
        // Passive income
        unclaimedMoney += incomePerSecond * speedMultiplier * Time.deltaTime;

        UpdateUnclaimedUI();

        // autosave
        saveTimer += Time.deltaTime;

        if (saveTimer >= SAVE_INTERVAL)
        {
            saveTimer = 0f;
            SaveData();
        }
    }

    public void AddMoney(int amount)
    {
        AddToUnclaimed(amount);
    }

    public void AddToWallet(int amount)
    {
        currentMoney += amount;

        SaveData();
        UpdateUI();
    }

    public void AddToUnclaimed(int amount)
    {
        unclaimedMoney += amount;

        UpdateUnclaimedUI();
        SaveData();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;

            SaveData();
            UpdateUI();

            return true;
        }

        return false;
    }

    public void ClaimAll()
    {
        int claim = Mathf.FloorToInt(unclaimedMoney);

        if (claim <= 0)
            return;

        currentMoney += claim;
        unclaimedMoney -= claim;

        SaveData();

        UpdateUI();
        UpdateUnclaimedUI();

        ShowPopup(claim);
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = currentMoney + " $";
    }

    void UpdateUnclaimedUI()
    {
        if (unclaimedText != null)
            unclaimedText.text = Mathf.FloorToInt(unclaimedMoney) + " $";
    }

    void SaveData()
    {
        PlayerPrefs.SetInt(KEY_MONEY, currentMoney);
        PlayerPrefs.SetFloat(KEY_UNCLAIMED, unclaimedMoney);

        PlayerPrefs.SetFloat(KEY_SPEED, speedMultiplier);
        PlayerPrefs.SetFloat(KEY_INCOME, incomePerSecond);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        currentMoney = PlayerPrefs.GetInt(KEY_MONEY, 0);
        unclaimedMoney = PlayerPrefs.GetFloat(KEY_UNCLAIMED, 0f);

        speedMultiplier = PlayerPrefs.GetFloat(KEY_SPEED, 1f);
        incomePerSecond = PlayerPrefs.GetFloat(KEY_INCOME, 1f);
    }

    public void ResetMoney()
    {
        PlayerPrefs.DeleteKey(KEY_MONEY);
        PlayerPrefs.DeleteKey(KEY_UNCLAIMED);
        PlayerPrefs.DeleteKey(KEY_SPEED);
        PlayerPrefs.DeleteKey(KEY_INCOME);

        PlayerPrefs.Save();

        currentMoney = 0;
        unclaimedMoney = 0f;
        speedMultiplier = 1f;
        incomePerSecond = 1f;

        UpdateUI();
        UpdateUnclaimedUI();

        if (gainedPopupText != null)
            gainedPopupText.gameObject.SetActive(false);
    }

    void ShowPopup(int amount)
    {
        if (gainedPopupText == null)
            return;

        StopAllCoroutines();
        StartCoroutine(PopupRoutine(amount));
    }

    IEnumerator PopupRoutine(int amount)
    {
        gainedPopupText.gameObject.SetActive(true);
        gainedPopupText.text = "+" + amount + " $";

        yield return new WaitForSeconds(1f);

        gainedPopupText.gameObject.SetActive(false);
    }

    public void ResetUpgradesOnly()
    {
        incomePerSecond = 1f;
        speedMultiplier = 1f;

        PlayerPrefs.DeleteKey(KEY_SPEED);
        PlayerPrefs.DeleteKey(KEY_INCOME);

        string[] upgradeIds =
        {
            "GTX_1050",
            "GPU_2060",
            "GPU_3070",
            "GPU_4080",
            "GPU_5080Ti"
        };

        foreach (var id in upgradeIds)
        {
            PlayerPrefs.DeleteKey("UPG_" + id);
        }

        PlayerPrefs.Save();
    }
}