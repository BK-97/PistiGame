using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    #region Params
    [HideInInspector]
    public List<Card> playedCards;
    [HideInInspector]
    public List<PlayerController> Players;
    [HideInInspector]
    public int playIndex;
    private int playerCount;

    private int currentTurn;
    private int lastCollectIndex;
    #endregion
    public void Initialize(int currentPlayerCount, List<PlayerController> currentPlayers)
    {
        playerCount = currentPlayerCount;

        playIndex = Random.Range(0, playerCount - 1);
        Players = currentPlayers;
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameLogic = this;
        }
    }
    private void OnEnable()
    {
        GameManager.OnRoundReady.AddListener(ReadyToPlay);
        GameManager.OnCalculateScore.AddListener(CalculateScore);

    }
    private void OnDisable()
    {
        GameManager.OnRoundReady.RemoveListener(ReadyToPlay);
        GameManager.OnCalculateScore.RemoveListener(CalculateScore);

    }
    public void AddPlayedCard(Card card)
    {
        card.transform.SetParent(transform, false);
        playedCards.Add(card);

        if (!isRoundReady)
            return;
        CalculateWinLose();
        NextPlayer();

    }
    bool isRoundReady;
    private void NextPlayer()
    {
        if (playIndex + 1 == playerCount)
        {
            playIndex = 0;
        }
        else
            playIndex++;

        currentTurn++;
        if (currentTurn == playerCount * 4)
        {
            CardManager.Instance.DealCard();
            currentTurn = 0;
        }
        else
        {
            ReadyToPlay();

        }
    }
    private void ReadyToPlay()
    {
        isRoundReady = true;
        Players[playIndex].CanClickOn();
        Players[playIndex].canPlay = true;

    }
    #region WinLose
    private void CalculateWinLose()
    {
        if (playedCards.Count == 1)
        {
            CardManager.OnCardPlayed.Invoke();
            return;
        }

        Card previouseCard = playedCards[playedCards.Count - 2];
        Card currentCard = playedCards[playedCards.Count - 1];
        if (CheckPisti(previouseCard, currentCard))
            PistiWin();
        else
        {
            if (previouseCard.value == currentCard.value)
            {
                MoveAllCardToWinner(playIndex);
            }
            else if (currentCard.value == 11)
            {
                MoveAllCardToWinner(playIndex);
            }
        }
    }

    private bool CheckPisti(Card previousCard, Card currentCard)
    {
        if (playedCards.Count == 2)
        {
            if (previousCard.value == currentCard.value)
                return true;

        }
        return false;
    }
    private void PistiWin()
    {
        playedCards[0].point += 5;
        playedCards[1].point += 5;
        MoveAllCardToWinner(playIndex);

    }
    private void MoveAllCardToWinner(int playIndex)
    {
        for (int i = 0; i < playedCards.Count; i++)
        {
            playedCards[i].MoveToCollectedDecks(Players[playIndex].collectedDeckParent);
        }
        lastCollectIndex = playIndex;

        playedCards.Clear();
    }
    private void CalculateScore()
    {
        MoveAllCardToWinner(lastCollectIndex);
        int maxCount = 0;
        int maxIndex = -1;

        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].collectedDeckParent.GetComponent<CardCollector>().collectedCards.Count > maxCount)
            {
                maxCount = Players[i].currentDeck.Count;
                maxIndex = i;
            }
        }

        Players[maxIndex].collectedDeckParent.GetComponent<CardCollector>().HasMoreCard();
        GameManager.OnGameEnd.Invoke();

    }
    #endregion

}
