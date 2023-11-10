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
    public static UnityEvent OnGameStatusChanged = new UnityEvent();
    public static BoolEvent OnPlayerWin = new BoolEvent();
    public static BoolEvent OnPlayerLose = new BoolEvent();
    #endregion
    public enum GameStatus { Menu, WaitForPlay, Play, ScoreBoard }
    [HideInInspector]
    public GameStatus gameStatus;

    public GameLogic GameLogic;
    public TextMeshProUGUI betText;

    [HideInInspector]
    public bool isGameStarted;
    [HideInInspector]
    public int currentPlayerCount;
    [HideInInspector]
    public int currentBet;

    [HideInInspector]
    public List<PlayerController> currentPlayers;
    [HideInInspector]
    public int currentWinCount;
    [HideInInspector]
    public int currentLoseCount;
    private void OnEnable()
    {
        OnGameStart.AddListener(GameStartPanels);
        OnRoundEnd.AddListener(() => ChangeGameStatus(GameStatus.WaitForPlay));
        OnRoundReady.AddListener(() => ChangeGameStatus(GameStatus.Play));
        OnGameEnd.AddListener(EndGame);
        OnPlayerWin.AddListener(PlayerWinLose);
        OnPlayerLose.AddListener(PlayerWinLose);
        OnGameStatusChanged.AddListener(StopAllPlayers);
    }
    private void OnDisable()
    {
        OnGameStart.RemoveListener(GameStartPanels);
        OnRoundEnd.RemoveListener(() => ChangeGameStatus(GameStatus.WaitForPlay));
        OnRoundReady.RemoveListener(() => ChangeGameStatus(GameStatus.Play));
        OnGameEnd.RemoveListener(EndGame);
        OnPlayerWin.RemoveListener(PlayerWinLose);
        OnPlayerLose.RemoveListener(PlayerWinLose);
        OnGameStatusChanged.RemoveListener(StopAllPlayers);
    }
    private void GameStartPanels(int playerNumber, int bet)
    {
        ChangeGameStatus(GameStatus.WaitForPlay);
        currentBet = bet;
        currentPlayers = UIManager.Instance.OnPlayerChairs(playerNumber);
        betText.text = "BET: " + currentBet;
        UIManager.Instance.PanelOnOff(gameStatus);

        Invoke("CardManagerInitalize", 0.5f);
        GameLogic.Initialize(playerNumber, currentPlayers);
        currentPlayerCount = playerNumber;
        isGameStarted = true;

    }
    private void ChangeGameStatus(GameStatus newStatus)
    {
        gameStatus = newStatus;
        OnGameStatusChanged.Invoke();
    }
    private void StopAllPlayers()
    {
        if (gameStatus != GameStatus.WaitForPlay)
            return;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayers[i].canPlay = false;
        }
    }
    private void CardManagerInitalize()
    {
        CardManager.Instance.Initialize();
    }
    public void EndGame()
    {
        ChangeGameStatus(GameStatus.ScoreBoard);
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
        if (isWin)
        {
            PlayerPrefs.SetInt(PrefsKeys.WinCount, PlayerPrefs.GetInt(PrefsKeys.WinCount, 0) + 1);
            ExchangeManager.Instance.AddCurrency(CurrencyType.Cash, currentBet);
        }
        else
        {
            PlayerPrefs.SetInt(PrefsKeys.LoseCount, PlayerPrefs.GetInt(PrefsKeys.LoseCount, 0) + 1);
            ExchangeManager.Instance.UseCurrency(CurrencyType.Cash, currentBet);
        }
        currentWinCount = PlayerPrefs.GetInt(PrefsKeys.WinCount, 0);
        currentLoseCount = PlayerPrefs.GetInt(PrefsKeys.LoseCount, 0);
    }
    public void RestartGame()
    {
        ChangeGameStatus(GameStatus.Menu);
        UIManager.Instance.PanelOnOff(gameStatus);
    }
}

public class IntEvent : UnityEvent<int> { }
public class IntIntEvent : UnityEvent<int, int> { }
public class BoolEvent : UnityEvent<bool> { }
public class ListEvent : UnityEvent<List<PlayerController>> { }