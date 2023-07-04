using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class CardManager : MonoBehaviour
{
    #region Singleton
    private static CardManager instance;

    public static CardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<CardManager>();
                    singletonObject.name = "CardManagerSingleton";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public GameObject cardPrefab;
    public Transform cardParent;
    public List<Sprite> heartsCards;
    public List<Sprite> clubsCards;
    public List<Sprite> diamondsCards;
    public List<Sprite> spadesCards;
    private List<Card> deck;
    public enum CardTypes { Hearts,Clubs,Diamonds,Spades,NUMBER_TYPES }
    private void Start()
    {
        if (cardParent.gameObject.activeInHierarchy)
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
        DealCards(); // Kartlarý daðýt
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
                    return 10;
                else
                    return 0;
            case 11:
                return 1;
        }
        return 0;
    }

    private void CreateCard(CardTypes type, int value, int puan)
    {
        GameObject newCardObject = Instantiate(cardPrefab, cardParent);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.Init(type, value, puan);

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
    
    private void DealCards()
    {
        cardParent.GetComponent<CardHolder>().cards = deck;
        cardParent.GetComponent<CardHolder>().Initialize();
        // Kartlarý oyunculara daðýtmak için gereken iþlemleri gerçekleþtirin
        // Örneðin, her oyuncuya 13 kart vererek baþlayabilirsiniz
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
