using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using TMPro;
public class GameManager : Singleton<GameManager>
{
    #region Events
    public static IntIntEvent OnGameStart = new IntIntEvent();
    public static ListEvent ScoreBoardEvent = new ListEvent();
    public static UnityEvent OnRoundReady = new UnityEvent();
    public static UnityEvent OnRoundEnd = new UnityEvent();
    public static UnityEvent OnCalculateScore = new UnityEvent();
    public static UnityEvent OnGameEnd = new UnityEvent();
    public static BoolEvent OnPlayerWin = new BoolEvent();
    public static BoolEvent OnPlayerLose = new BoolEvent();
    #endregion
    public enum GameStatus { Menu,WaitForPlay, Play ,ScoreBoard}
    public GameStatus gameStatus;
    [HideInInspector]
    public List<PlayerController> currentPlayers;
    public GameLogic GameLogic;
    public bool isGameStarted;
    public int currentPlayerCount;
    public int currentBet;
    public TextMeshProUGUI betText;
    private void OnEnable()
    {
        OnGameStart.AddListener(GameStartPanels);
        OnRoundEnd.AddListener(() => gameStatus = GameStatus.WaitForPlay);
        OnRoundReady.AddListener(()=>gameStatus=GameStatus.Play);
        OnGameEnd.AddListener(EndGame);
        OnPlayerWin.AddListener(PlayerWinLose);
        OnPlayerLose.AddListener(PlayerWinLose);

    }
    private void OnDisable()
    {
        OnGameStart.RemoveListener(GameStartPanels);
        OnRoundEnd.RemoveListener(() => gameStatus = GameStatus.WaitForPlay);
        OnRoundReady.RemoveListener(() => gameStatus = GameStatus.Play);
        OnGameEnd.RemoveListener(EndGame);
        OnPlayerWin.RemoveListener(PlayerWinLose);
        OnPlayerLose.RemoveListener(PlayerWinLose);

    }
    private void GameStartPanels(int playerNumber,int bet)
    {
        gameStatus = GameStatus.WaitForPlay;
        currentBet = bet;
        currentPlayers =UIManager.Instance.OnPlayerChairs(playerNumber);
        betText.text = "BET: " + currentBet;
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


    private void PlayerWinLose(bool isWin)
    {
        if(isWin)  
        {
            PlayerPrefs.SetInt(PrefsKeys.WinCount,PlayerPrefs.GetInt(PrefsKeys.WinCount,0)+1);
            PlayerPrefs.SetInt(PrefsKeys.Cash,PlayerPrefs.GetInt(PrefsKeys.Cash,1000)+currentBet);

        }
        else
        {
            PlayerPrefs.SetInt(PrefsKeys.LoseCount, PlayerPrefs.GetInt(PrefsKeys.LoseCount, 0) + 1);
            if(PlayerPrefs.GetInt(PrefsKeys.Cash, 1000) - currentBet<0)
                PlayerPrefs.SetInt(PrefsKeys.Cash, 0);
            else
                PlayerPrefs.SetInt(PrefsKeys.Cash, PlayerPrefs.GetInt(PrefsKeys.Cash, 1000) - currentBet);

        }

    }
    public void RestartGame()
    {
        gameStatus = GameStatus.Menu;
        UIManager.Instance.PanelOnOff(gameStatus);
        
    }
}

public class IntEvent : UnityEvent<int> { }
public class IntIntEvent : UnityEvent<int,int> { }
public class BoolEvent : UnityEvent<bool> { }
public class ListEvent : UnityEvent<List<PlayerController>> { }