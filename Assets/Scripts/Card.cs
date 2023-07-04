using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Card : MonoBehaviour
{
    public CardManager.CardTypes type;
    public int value;
    public int puan;
    public bool canBeUse;

    public Image image;
    public Transform PlayGroundHolder;
    public void Init(CardManager.CardTypes cardType, int cardValue, int cardPuan,Transform playGround)
    {
        type = cardType;
        value = cardValue;
        puan = cardPuan;
        PlayGroundHolder = playGround;
        UpdateCardSprite();
    }
    private void OnEnable()
    {
        GameManager.OnRoundEnd.AddListener(()=>canBeUse=false);
        GameManager.OnRoundReady.AddListener(() => canBeUse = true);
    }
    private void OnDisable()
    {
        GameManager.OnRoundEnd.RemoveListener(() => canBeUse = false);
        GameManager.OnRoundReady.RemoveListener(() => canBeUse = true);
    }
    private void UpdateCardSprite()
    {
        image.sprite=CardManager.Instance.GetCardSprite(value,type);
    }
    public void ReverseCard()
    {
        image.gameObject.SetActive(false);
    }
    public void Move(Transform targetTransform,float moveSpeed)
    {
        transform.DOMove(targetTransform.position, moveSpeed).OnComplete(() => AddDeck(targetTransform));
    }
    private void AddDeck(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        if (parentTransform == PlayGroundHolder)
            return;
        GetComponentInParent<PlayerController>().CardAdd(this);

    }
    public void UseCard()
    {
        if (!canBeUse)
            return;
        canBeUse = false;
        Move(PlayGroundHolder,0.5f);
        GetComponentInParent<PlayerController>().RemoveCard(this);
        GameManager.OnCardUsed.Invoke();
    }
}
