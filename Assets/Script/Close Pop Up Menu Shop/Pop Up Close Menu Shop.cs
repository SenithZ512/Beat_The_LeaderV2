using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCloseMenuShop : MonoBehaviour
{
    public GameObject shopPanel;

    public void Close()
    {
        shopPanel.SetActive(false);
    }
    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
}