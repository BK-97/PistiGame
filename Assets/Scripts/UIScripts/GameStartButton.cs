using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    private TableCreator tableCreator;
    int playerCount=2;
    private int bet;
    public void SetTableCreatorParent(TableCreator creator)
    {
        tableCreator = creator;
        bet = tableCreator.currentBet;
        playerCount = tableCreator.currentPlayerCount;
        StartGame();
    }
    private void StartGame()
    {
        GameManager.OnGameStart.Invoke(playerCount, bet);
    }

}
