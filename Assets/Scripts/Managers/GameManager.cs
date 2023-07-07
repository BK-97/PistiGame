using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class GameManager : Singleton<GameManager>
{
    #region Events
    public static IntEvent OnGameStart = new IntEvent();
    public static ListEvent ScoreBoardEvent = new ListEvent();
    public static UnityEvent OnRoundReady = new UnityEvent();
    public static UnityEvent OnRoundEnd = new UnityEvent();
    public static UnityEvent OnCalculateScore = new UnityEvent();
    public static UnityEvent OnGameEnd = new UnityEvent();
    #endregion
    public enum GameStatus { Menu,WaitForPlay, Play ,ScoreBoard}
    public GameStatus gameStatus;
    [HideInInspector]
    public List<PlayerController> currentPlayers;
    public GameLogic GameLogic;
    public bool isGameStarted;
    public int currentPlayerCount;
    private void OnEnable()
    {
        OnGameStart.AddListener(GameStartPanels);
        OnRoundEnd.AddListener(() => gameStatus = GameStatus.WaitForPlay);
        OnRoundReady.AddListener(()=>gameStatus=GameStatus.Play);
        OnGameEnd.AddListener(EndGame);
    }
    private void OnDisable()
    {
        OnGameStart.RemoveListener(GameStartPanels);
        OnRoundEnd.RemoveListener(() => gameStatus = GameStatus.WaitForPlay);
        OnRoundReady.RemoveListener(() => gameStatus = GameStatus.Play);
        OnGameEnd.RemoveListener(EndGame);
    }
    private void GameStartPanels(int playerNumber)
    {
        gameStatus = GameStatus.WaitForPlay;
        currentPlayers=UIManager.Instance.OnPlayerChairs(playerNumber);

        UIManager.Instance.PanelOnOff(gameStatus);

        Invoke("CardManagerInitalize", 0.5f);
        GameLogic.Initialize(playerNumber, currentPlayers);
        currentPlayerCount= playerNumber;
        isGameStarted = true;
    }
    private void Update()
    {
        if (gameStatus == GameStatus.WaitForPlay)
            StopAllPlayers();
    }
    private void StopAllPlayers()
    {
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayers[i].canPlay=false;
        }
    }
    private void CardManagerInitalize()
    {
        CardManager.Instance.Initialize();
    }
    public void EndGame()
    {
        gameStatus = GameStatus.ScoreBoard;
        UIManager.Instance.PanelOnOff(gameStatus);
        ScoreBoardEvent.Invoke(SortPlayersByPoint());

    }
    public List<PlayerController> SortPlayersByPoint()
    {
        List<PlayerController> sortedPlayers = currentPlayers.OrderByDescending(player => player.point).ToList();
        return sortedPlayers;
    }
}

public class IntEvent : UnityEvent<int> { }
public class ListEvent : UnityEvent<List<PlayerController>> { }