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

    public List<GameObject> otherPanels;

    private void OnEnable()
    {
        OnGameStart.AddListener(GameStartPanels);
    }
    private void OnDisable()
    {
        OnGameStart.RemoveListener(GameStartPanels);

    }
    private void GameStartPanels(int playerCount)
    {
        for (int i = 0; i < otherPanels.Count; i++)
        {
            otherPanels[i].SetActive(false);
        }
    }
}

public class IntEvent : UnityEvent<int> { }