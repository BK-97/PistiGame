using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = "GameManagerSingleton";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public static IntEvent OnGameStart = new IntEvent();
    public static UnityEvent OnRoundReady = new UnityEvent();
    public static UnityEvent OnRoundEnd = new UnityEvent();
    public static UnityEvent OnCardUsed = new UnityEvent();
    public List<GameObject> otherPanels;
    public List<PlayerController> players;
    public GameObject gamePanel;
    public int playerCount;
    public int currentPlayIndex;
    private void OnEnable()
    {
        OnGameStart.AddListener(GameStartPanels);
        OnCardUsed.AddListener(NextPlayer);
        OnRoundReady.AddListener(() => players[currentPlayIndex].CanPlay());
        OnRoundEnd.AddListener(StopAllPlayers);

    }
    private void OnDisable()
    {
        OnGameStart.RemoveListener(GameStartPanels);
        OnCardUsed.RemoveListener(NextPlayer);
        OnRoundReady.RemoveListener(() => players[currentPlayIndex].CanPlay());
        OnRoundEnd.RemoveListener(StopAllPlayers);

    }
    private void GameStartPanels(int playerNumber)
    {
        playerCount = playerNumber;
        for (int i = 0; i < otherPanels.Count; i++)
        {
            otherPanels[i].SetActive(false);
        }
        gamePanel.SetActive(true);
        Invoke("CardManagerInitalize", 0.5f);

        currentPlayIndex = Random.Range(0, playerCount - 1);
    }
    private void NextPlayer()
    {
        players[currentPlayIndex].CantPlay();
        if (currentPlayIndex + 1 == playerCount)
            currentPlayIndex = 0;
        else
            currentPlayIndex++;
        players[currentPlayIndex].CanPlay();

    }
    private void StopAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].CantPlay();
        }
    }
    public void PlayerAdd(PlayerController player)
    {
        players.Add(player);
    }
    private void CardManagerInitalize()
    {
        CardManager.Instance.Initialize();
    }
}

public class IntEvent : UnityEvent<int> { }