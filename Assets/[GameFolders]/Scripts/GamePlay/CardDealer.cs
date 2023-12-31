using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardDealer : MonoBehaviour
{
    #region Params
    public List<Card> cards;
    public Transform playGroundHolder;
    public TextMeshProUGUI cardCountText;
    public List<Transform> PlayersHolders;
    public GameLogic gameLogic;
    public Transform cardParent;
    #endregion
    public void Initialize()
    {
        cardCountText.text = cards.Count.ToString();

        List<Transform> tempHolders = new List<Transform>();
        for (int i = gameLogic.playIndex; i < PlayersHolders.Count; i++)
        {
            tempHolders.Add(PlayersHolders[i]);
        }

        for (int i = 0; i < gameLogic.playIndex; i++)
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
            MoveOnPlayGroundHolder(cards[0], playGroundHolder,true);
            yield return new WaitForSeconds(0.2f);
        }
        MoveOnPlayGroundHolder(cards[0], playGroundHolder,false);
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

        List<Card> tempList = new List<Card>(cards);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < PlayersHolders.Count; j++)
            {
                if (tempList.Count > 0)
                {
                    Card card = tempList[0];
                    tempList.RemoveAt(0);
                    bool status = false;
                    if (j != 0)
                        status=true;

                    StartCoroutine(MoveCardToPlayer(card, PlayersHolders[j] ,(i+1) * 0.2f, status));
                }
            }
        }
        Invoke("DealEnd",1);
    }
    private IEnumerator MoveCardToPlayer(Card card, Transform playerHolder, float delay,bool isReversed)
    {
        yield return new WaitForSeconds(delay);
        MoveOnPlayGroundHolder(card, playerHolder, isReversed);

    }
    private void MoveOnPlayGroundHolder(Card moverCard,Transform target,bool isReversed)
    {
        moverCard.Move(target, CardManager.Instance.cardMoveSpeed, isReversed);
        RemoveCard(moverCard);

    }
    private void RemoveCard(Card removeThis)
    {
        cards.Remove(removeThis);
        cardCountText.text = cards.Count.ToString();
    }
    private void DealEnd()
    {
        GameManager.OnRoundReady.Invoke();
    }
}
