using TMPro;
using UnityEngine;

public class UpgradeOwnText : MonoBehaviour
{
    public string upgradeID;
    public TextMeshProUGUI text;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (UpgradeManager.Instance.IsBought(upgradeID))
            text.text = "Owned";
        else
            text.text = "0";
    }
}