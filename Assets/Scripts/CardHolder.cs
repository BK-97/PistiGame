using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardHolder : MonoBehaviour
{
    public List<Card> cards;
    public Transform playGroundHolder;
    public TextMeshProUGUI cardCountText;

    public void Initialize()
    {
        cardCountText.text = cards.Count.ToString();
        CallReverseCards();
    }
    private IEnumerator CallReverseCardsCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            ReverseCards();
            yield return new WaitForSeconds(0.2f);
        }
        MoveOnPlayGroundHolder(cards[0]);
    }

    private void CallReverseCards()
    {
        StartCoroutine(CallReverseCardsCoroutine());
    }
    private void ReverseCards()
    {
        cards[0].ReverseCard();
        MoveOnPlayGroundHolder(cards[0]);
    }
    private void MoveOnPlayGroundHolder(Card moverCard)
    {
        moverCard.Move(playGroundHolder);
        RemoveCard(cards[0]);
    }
    private void RemoveCard(Card removeThis)
    {
        cards.Remove(removeThis);
        cardCountText.text = cards.Count.ToString();
    }
}
