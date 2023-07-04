using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardHolder : MonoBehaviour
{
    public List<Card> cards;
    public Transform playGroundHolder;
    public TextMeshProUGUI cardCountText;
    public List<Transform> PlayersHolders;

    public void Initialize()
    {
        cardCountText.text = cards.Count.ToString();

        List<Transform> tempHolders = new List<Transform>();
        for (int i = GameManager.Instance.currentPlayIndex; i < PlayersHolders.Count; i++)
        {
            tempHolders.Add(PlayersHolders[i]);
        }

        for (int i = 0; i < GameManager.Instance.currentPlayIndex; i++)
        {
            tempHolders.Add(PlayersHolders[i]);
        }
        CallReverseCards();
    }
    #region StartDeal
    private IEnumerator CallReverseCardsCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            cards[0].ReverseCard();
            MoveOnPlayGroundHolder(cards[0], playGroundHolder);
            yield return new WaitForSeconds(0.2f);
        }
        MoveOnPlayGroundHolder(cards[0], playGroundHolder);
        yield return new WaitForSeconds(0.1f);
        DealToPlayer();
    }
    private void CallReverseCards()
    {
        StartCoroutine(CallReverseCardsCoroutine());
    }
    #endregion

    public void DealToPlayer()
    {
        GameManager.OnRoundEnd.Invoke();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < PlayersHolders.Count; j++)
            {
                if (cards.Count > 0)
                {
                    Card card = cards[0];
                    cards.RemoveAt(0);
                    if (j != 0)
                        card.ReverseCard();
                    StartCoroutine(MoveCardToPlayer(card, PlayersHolders[j] ,(i+1) * 0.2f));
                }
            }
        }
        Invoke("DealEnd",1);
    }
    private void DealEnd()
    {
        GameManager.OnRoundReady.Invoke();

    }
    private IEnumerator MoveCardToPlayer(Card card, Transform playerHolder, float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveOnPlayGroundHolder(card, playerHolder);

    }
    private void MoveOnPlayGroundHolder(Card moverCard,Transform target)
    {
        moverCard.Move(target, CardManager.Instance.cardMoveSpeed);
        RemoveCard(moverCard);
    }
    private void RemoveCard(Card removeThis)
    {
        cards.Remove(removeThis);
        cardCountText.text = cards.Count.ToString();
    }

}
