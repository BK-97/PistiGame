using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cashText;
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private TextMeshProUGUI loseText;
    private void OnEnable()
    {
        GameManager.OnPlayerWin.AddListener(UpdateText);
        GameManager.OnPlayerLose.AddListener(UpdateText);
    }
    private void OnDisable()
    {
        GameManager.OnPlayerWin.RemoveListener(UpdateText);
        GameManager.OnPlayerLose.RemoveListener(UpdateText);
    }
    private void Start()
    {
        UpdateText(true);
    }
    private void UpdateText(bool status)
    {
        cashText.text = ExchangeManager.Instance.GetCurrency(CurrencyType.Cash).ToString();
        winText.text = GameManager.Instance.currentWinCount.ToString();
        loseText.text = GameManager.Instance.currentLoseCount.ToString();
    }

}
