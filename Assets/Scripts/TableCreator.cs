using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TableCreator : MonoBehaviour
{
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI currentText;

    private int minBet;
    private int maxBet;
    private int cash;
    public int currentBet;
    public Slider slider;
    

    public Button twoPlayer;
    public Button fourPlayer;
    private Button lastSelectedButton;
    public int currentPlayerCount;

    void Start()
    {
        cash = PlayerPrefs.GetInt(PrefsKeys.Cash,1000);
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
        if (slider.value > cash)
            slider.value = cash;
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
