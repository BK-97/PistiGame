using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableController : MonoBehaviour
{
    public List<Button> buttons;
    public int maxBet;
    public int minBet;
    [SerializeField]
    private TextMeshProUGUI betTMP;
    [SerializeField]
    private string tableNameText;
    [SerializeField]
    private TextMeshProUGUI tableNameTMP;

    private void Start()
    {
        if (ExchangeManager.Instance.GetCurrency(CurrencyType.Cash) < minBet)
            CloseAllButtons();
        else
            OpenAllButtons();
        TableSet();
    }
    private void TableSet()
    {
        betTMP.text = "Bet Range: "+minBet.ToString() + "-" + maxBet.ToString();
        tableNameTMP.text = tableNameText.ToString();
    }
    private void CloseAllButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
    }
    private void OpenAllButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
