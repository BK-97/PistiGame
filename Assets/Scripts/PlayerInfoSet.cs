using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInfoSet : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerCash;
    private void OnEnable()
    {
        ExchangeManager.Instance.OnCurrencyChange.AddListener(SetInfo);
    }
    private void OnDisable()
    {
        ExchangeManager.Instance.OnCurrencyChange.AddListener(SetInfo);
    }
    private void SetInfo(Dictionary<CurrencyType,int> newCurrency)
    {
        playerName.text = PlayerPrefs.GetString(PrefsKeys.PlayerName, "Player");
        playerCash.text = newCurrency[CurrencyType.Cash].ToString();
    }
}
