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
    public TextMeshProUGUI betText;

    private void Start()
    {
        if (PlayerPrefs.GetInt(PrefsKeys.Cash, 1000) < minBet)
            CloseAllButtons();
        else
            OpenAllButtons();
        BetSet();
    }
    private void BetSet()
    {
        betText.text = "Bet Range: "+minBet.ToString() + "-" + maxBet.ToString();
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
