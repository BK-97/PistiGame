using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Card : MonoBehaviour
{
    public CardManager.CardTypes type;
    public int value;
    public int point;
    public bool canBeClicked;
    public Image image;
    public Transform PlayGroundHolder;
    public void Init(CardManager.CardTypes cardType, int cardValue, int cardPuan,Transform playGround)
    {
        type = cardType;
        value = cardValue;
        point = cardPuan;
        PlayGroundHolder = playGround;
        UpdateCardSprite();
    }

    private void UpdateCardSprite()
    {
        image.sprite=CardManager.Instance.GetCardSprite(value,type);
    }
    public void ReverseCard(bool reverse)
    {
        image.gameObject.SetActive(!reverse);
    }
    public void Move(Transform targetTransform,float moveSpeed)
    {
        transform.DOMove(targetTransform.position, moveSpeed).OnComplete(() => AddDeck(targetTransform));
    }
    public void MoveToCollectedDecks(Transform targetTransform)
    {
        transform.DOMove(targetTransform.position, 0.2f).OnComplete(() => transform.SetParent(targetTransform));

    }
    private void AddDeck(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        if (parentTransform == PlayGroundHolder)
        {
            PlayGroundHolder.gameObject.GetComponent<GameLogic>().AddPlayedCard(this);
        }
        else
            GetComponentInParent<PlayerController>().CardAdd(this);

    }
    public void ClickCard()
    {
        if (!canBeClicked)
            return;

        canBeClicked = false;
        UseCard();
    }
    public void UseCard()
    {
        Move(PlayGroundHolder,0.5f);
        GetComponentInParent<PlayerController>().RemoveCard(this);
    }
}
