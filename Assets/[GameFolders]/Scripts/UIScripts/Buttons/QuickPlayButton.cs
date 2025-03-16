using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickPlayButton : MonoBehaviour
{
    [SerializeField]
    private TableController tableController;
    private int quickBet;
    private void OnEnable()
    {
        if (tableController.minBet == 0)
        {
            if(ExchangeManager.Instance.GetCurrency(CurrencyType.Cash)>=Mathf.Round(tableController.maxBet/2))
                quickBet = tableController.maxBet / 2;
            else
                quickBet = ExchangeManager.Instance.GetCurrency(CurrencyType.Cash);
        }
        else
            quickBet = tableController.minBet;
    }
    public void StartGame()
    {
        ExchangeManager.Instance.UseCurrency(CurrencyType.Cash, quickBet);
        GameManager.OnGameStart.Invoke(2, quickBet);
    }
}
