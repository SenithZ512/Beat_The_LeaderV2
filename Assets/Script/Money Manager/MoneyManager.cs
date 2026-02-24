using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney = 0;
    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadMoney();
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        SaveMoney();
        UpdateUI();
    }

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

    void UpdateUI()
    {
        moneyText.text = currentMoney + " $";
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();
    }

    void LoadMoney()
    {
        currentMoney = PlayerPrefs.GetInt("Money", 0);
    }
    public void ResetMoney()
    {
        PlayerPrefs.DeleteAll();
        currentMoney = 0;
        UpdateUI();
    }
}