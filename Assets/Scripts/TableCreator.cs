using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TableCreator : MonoBehaviour
{
    #region Params
    [Header("Texts")]
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI currentText;

    [Space(5)]
    [Header("BetInfos")]
    public Slider slider;
    [HideInInspector]
    public int currentBet;
    private int minBet;
    private int maxBet;

    [Space(5)]
    [Header("Selection Area")]
    public Button twoPlayer;
    public Button fourPlayer;

    [HideInInspector]
    public int currentPlayerCount;
    private Button lastSelectedButton;

    #endregion
    void Start()
    {
        OnButtonSelected(twoPlayer);
    }
    public void BetSetter(int minbet,int maxbet)
    {
        minBet = minbet;
        maxBet = maxbet;
        Initialize();
    }
    private void Initialize()
    {
        minText.text = minBet.ToString();
        maxText.text = maxBet.ToString();
        slider.minValue = minBet;
        slider.maxValue = maxBet;
        slider.value = (slider.minValue + slider.maxValue) * 0.5f;
        currentBet= (int)((slider.minValue + slider.maxValue) * 0.5f);
    }
    void Update()
    {
        if (slider.value > ExchangeManager.Instance.GetCurrency(CurrencyType.Cash))
            slider.value = ExchangeManager.Instance.GetCurrency(CurrencyType.Cash);
        currentBet = (int)slider.value;
        currentText.text = currentBet.ToString();
    }

    public void OnButtonSelected(Button selectedButton)
    {
        if (lastSelectedButton != null)
        {
            lastSelectedButton.interactable = true;
        }

        selectedButton.interactable = false; 
        lastSelectedButton = selectedButton;
        if (lastSelectedButton == twoPlayer)
            currentPlayerCount = 2;
        else
            currentPlayerCount = 4;
    }
    public void CreateTable()
    {
        GetComponentInChildren<GameStartButton>().SetTableCreatorParent(this);

    }
}
