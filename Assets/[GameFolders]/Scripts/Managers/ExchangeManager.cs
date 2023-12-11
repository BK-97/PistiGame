using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CurrencyType
{
    Cash
}

public class ExchangeManager : Singleton<ExchangeManager>
{
    #region Params
    private Dictionary<CurrencyType, int> currencyDictionary;
    public DictonaryEvent OnCurrencyChange = new DictonaryEvent();
    [HideInInspector]
    public UnityEvent OnCurrencyAdded = new UnityEvent();
    const int STARTER_COIN = 1000;
    #endregion
    #region StarterMethods
    public ExchangeManager()
    {
        currencyDictionary = new Dictionary<CurrencyType, int>();
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt(PrefsKeys.Cash, STARTER_COIN) < STARTER_COIN)
            PlayerPrefs.SetInt(PrefsKeys.Cash, STARTER_COIN);

        currencyDictionary[CurrencyType.Cash] = PlayerPrefs.GetInt(PrefsKeys.Cash, STARTER_COIN);
    }
    #endregion
    #region CurrencyMethods
    public bool UseCurrency(CurrencyType currencyType, int amount)
    {
        if (currencyDictionary.ContainsKey(currencyType))
        {
            if (currencyDictionary[currencyType] >= amount)
            {
                currencyDictionary[currencyType] -= amount;
                PlayerPrefs.SetInt(PrefsKeys.Cash, currencyDictionary[currencyType]);
                OnCurrencyChange.Invoke(currencyDictionary);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void AddCurrency(CurrencyType currencyType, int amount)
    {
        if (currencyDictionary.ContainsKey(currencyType))
        {
            currencyDictionary[currencyType] += amount;
            PlayerPrefs.SetInt(PrefsKeys.Cash, currencyDictionary[currencyType]);
            OnCurrencyChange.Invoke(currencyDictionary);
            OnCurrencyAdded.Invoke();
        }
    }

    public int GetCurrency(CurrencyType currencyType)
    {
        if (currencyDictionary.ContainsKey(currencyType))
        {
            return currencyDictionary[currencyType];
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
public class DictonaryEvent : UnityEvent<Dictionary<CurrencyType, int>> { }