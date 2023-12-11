using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    #region Params
    public enum PlayerTypes { Player,Bot1,Bot2,Bot3}
    public PlayerTypes playerType;
    public Transform collectedDeckParent;
    public Transform deckParent;

    [HideInInspector]
    public List<Card> currentDeck;
    [HideInInspector]
    public GameLogic gameLogic;
    [HideInInspector]
    public bool canPlay;
    [HideInInspector]
    public int point;

    [SerializeField]
    private TextMeshProUGUI currentCashTMP;
    #endregion
    private void OnEnable()
    {
        if (currentCashTMP != null)
            currentCashTMP.text = ExchangeManager.Instance.GetCurrency(CurrencyType.Cash).ToString();
    }
    public void CardAdd(Card addThis)
    {
        currentDeck.Add(addThis);
        addThis.gameObject.transform.localRotation = Quaternion.identity;

        if (playerType != PlayerTypes.Player)
            return;

        addThis.canBeClicked = true;
    }
    public void RemoveCard(Card removeThis)
    {
        removeThis.canBeClicked = false;
        currentDeck.Remove(removeThis);
        CanClickOff();
    }
    public void CanClickOff()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            currentDeck[i].canBeClicked = false;
        }
    }
    public void CanClickOn()
    {
        if (playerType != PlayerTypes.Player)
        {
            PlayAI();
        }
        else
        {
            for (int i = 0; i < currentDeck.Count; i++)
            {
                currentDeck[i].canBeClicked = true;
            }
        }

    }
    private void PlayAI()
    {
        if(gameLogic.playedCards.Count==0)
        {
            Card ChoosenCard = null;
            for (int i = 0; i < currentDeck.Count; i++)
            {
                if (currentDeck[i].value != 11)
                    ChoosenCard = currentDeck[i];
            } 
            ChoosenCard.ReverseCard(false);
            ChoosenCard.UseCard();
        }
        else
        {
            int lastCardValue = gameLogic.playedCards[gameLogic.playedCards.Count - 1].value;

            Card ChoosenCard = HasSameValueCard(lastCardValue);
            ChoosenCard.ReverseCard(false);
            ChoosenCard.UseCard();
        }

        CanClickOff();
    }
    Card HasSameValueCard(int lastValue)
    {
        Card choosenCard =null;
        for (int i = 0; i < currentDeck.Count; i++)
        {
            if (currentDeck[i].value == lastValue)
                choosenCard = currentDeck[i];
        }
        if (choosenCard != null)
            return choosenCard;
        if (choosenCard == null)
        {
            for (int i = 0; i < currentDeck.Count; i++)
            {
                if (currentDeck[i].value == 11)
                    choosenCard = currentDeck[i];
            }
        }
        if (choosenCard == null)
            return currentDeck[0];
        else
            return choosenCard;
    }
}
