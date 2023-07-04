using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardManager.CardTypes type;
    public int value;
    public int puan;

    public Image image;

    public void Init(CardManager.CardTypes cardType, int cardValue, int cardPuan)
    {
        type = cardType;
        value = cardValue;
        puan = cardPuan;

        UpdateCardSprite();
    }

    private void UpdateCardSprite()
    {
        image.sprite=CardManager.Instance.GetCardSprite(value,type);
    }
    public void ReverseCard()
    {
        image.gameObject.SetActive(false);
    }
}
