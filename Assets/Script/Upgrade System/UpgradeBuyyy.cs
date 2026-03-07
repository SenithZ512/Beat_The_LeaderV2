using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBuyyy : MonoBehaviour
{
    public string upgradeId;

    public Button buyButton;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI stateText;

    private void Start()
    {
        RefreshUI();
    }

    public void Buy()
    {
        if (UpgradeManager.Instance.TryBuy(upgradeId))
        {
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        bool bought = UpgradeManager.Instance.IsBought(upgradeId);

        if (bought)
        {
            buyButton.interactable = false;

            if (stateText != null)
                stateText.text = "OWNED";
        }
        else
        {
            buyButton.interactable = true;

            var def = GetDef();

            if (priceText != null && def != null)
                priceText.text = def.price + "$";
        }
    }

    UpgradeManager.UpgradeDef GetDef()
    {
        foreach (var u in UpgradeManager.Instance.upgrades)
        {
            if (u.id == upgradeId)
                return u;
        }

        return null;
    }
}
