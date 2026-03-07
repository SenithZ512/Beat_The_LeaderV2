using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (MoneyManager.Instance != null)
        {
            text.text = MoneyManager.Instance.currentMoney + " $";
        }
    }
}