using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    public int playerCount = 2;
    public void StartGame()
    {
        GameManager.OnGameStart.Invoke(playerCount);
    }
    public void ChangePlayerCount(int currentCount)
    {
        playerCount = currentCount;
    }
}
