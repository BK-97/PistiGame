using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class CardManager : Singleton<CardManager>
{
    public GameObject cardPrefab;
    public CardHolder cardHolder;
    public Transform playGroundTransform;
    public List<Sprite> heartsCards;
    public List<Sprite> clubsCards;
    public List<Sprite> diamondsCards;
    public List<Sprite> spadesCards;
    private List<Card> deck;

    public float cardMoveSpeed;
    public static UnityEvent OnCardPlayed = new UnityEvent();

    public enum CardTypes { Hearts,Clubs,Diamonds,Spades,NUMBER_TYPES }
    private void Start()
    {
        if (cardHolder.gameObject.activeInHierarchy)
            Initialize();
    }
    public void Initialize()
    {
        deck = new List<Card>();

        for (int type = 0; type < (int)CardTypes.NUMBER_TYPES; type++)
        {
            for (int value = 1; value <= 13; value++)
            {
                int puan = GetCardPuan(value, type);
                CardTypes cardType = (CardTypes)type;
                CreateCard(cardType, value, puan);
            }
        }

        ShuffleDeck();
        HolderInitialize();
    }

    private int GetCardPuan(int value,int type)
    {
        switch (value)
        {
            case 1:
                return 1;
            case 2:
                if (type == 3)
                    return 2;
                else
                    return 0;
            case 10:
                if (type == 2)
                    return 3;
                else
                    return 0;
            case 11:
                return 1;
        }
        return 0;
    }

    private void CreateCard(CardTypes type, int value, int puan)
    {
        GameObject newCardObject = Instantiate(cardPrefab, cardHolder.gameObject.transform);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.Init(type, value, puan, playGroundTransform);

        deck.Add(newCard);
    }

    private void ShuffleDeck()
    {
        int deckSize = deck.Count;
        System.Random random = new System.Random();

        for (int i = deckSize - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    public void DealCard()
    {
        if (cardHolder.cards.Count == 0)
        {
            GameManager.OnCalculateScore.Invoke();
        }
        else
            cardHolder.DealToPlayer();

    }
    private void HolderInitialize()
    {
        
        for (int i = 0; i < GameManager.Instance.currentPlayers.Count; i++)
        {
            cardHolder.PlayersHolders.Add(GameManager.Instance.currentPlayers[i].deckParent);

        }
        cardHolder.cards = deck;
        cardHolder.Initialize();

    }
    public Sprite GetCardSprite(int value, CardTypes type)
    {
        List<Sprite> spriteList = GetSpriteListByType(type);
        if (spriteList != null)
        {
            if (value >= 1 && value <= spriteList.Count)
            {
                return spriteList[value - 1];
            }
        }

        return null;
    }

    private List<Sprite> GetSpriteListByType(CardTypes type)
    {
        switch (type)
        {
            case CardTypes.Spades:
                return spadesCards;
            case CardTypes.Hearts:
                return heartsCards;
            case CardTypes.Diamonds:
                return diamondsCards;
            case CardTypes.Clubs:
                return clubsCards;
            default:
                return null;
        }
    }
}
