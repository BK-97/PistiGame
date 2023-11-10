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
    public Image frontSide;
    public GameObject backSide;
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
        frontSide.sprite=CardManager.Instance.GetCardSprite(value,type);
    }
    public void ReverseCard(bool reverse)
    {
        backSide.SetActive(reverse);
    }
    public void Move(Transform targetTransform,float moveSpeed,bool isReversed)
    {
        transform.DOMove(targetTransform.position, moveSpeed).OnComplete(() => CardMoveEnd(isReversed,targetTransform));
    }
    private void CardMoveEnd(bool isReversed,Transform targetTransform)
    {
        ReverseCard(isReversed);
        AddDeck(targetTransform);
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
    private void RandomRotate(float rotateTime)
    {
        float randomAngle = Random.Range(-180f, 180f);
        transform.DORotate(new Vector3(0f, 0f, randomAngle), rotateTime, RotateMode.Fast)
            .SetEase(Ease.Linear);
    }
    public void UseCard()
    {
        float moveSpeed = 0.5f;
        Move(PlayGroundHolder, moveSpeed, false);
        RandomRotate(moveSpeed);
        GetComponentInParent<PlayerController>().RemoveCard(this);
    }
}
