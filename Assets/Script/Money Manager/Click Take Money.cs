using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTakeMoney : MonoBehaviour
{
    public int amount = 10;

    public void GiveMoney()
    {
        MoneyManager.Instance.AddMoney(amount);
    }
}