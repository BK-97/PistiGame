using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    public GameObject menuPanel;
    public GameObject playerInfoPanel;
    public GameObject createTablePanel;
    public GameObject gamePanel;
    public List<GameObject> playerChairs;
    public GameObject scoreBoardPanel;

    private void Start()
    {
        OffPlayerChairs();
    }
    public void PanelOnOff(GameManager.GameStatus gameState)
    {
        switch (gameState)
        {
            case GameManager.GameStatus.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                createTablePanel.SetActive(false);
                scoreBoardPanel.SetActive(false);
                break;
            case GameManager.GameStatus.WaitForPlay:
                menuPanel.SetActive(false);
                createTablePanel.SetActive(false);
                gamePanel.SetActive(true);
                break;
            case GameManager.GameStatus.Play:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true); 
                break;
            case GameManager.GameStatus.ScoreBoard:
                scoreBoardPanel.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void CreateTablePanel(int minBet,int maxBet)
    {
        createTablePanel.SetActive(true);
        createTablePanel.GetComponent<TableCreator>().BetSetter(minBet, maxBet);
        menuPanel.SetActive(false);

    }
    public void OffPlayerChairs()
    {
        for (int i = 0; i < playerChairs.Count; i++)
        {
            playerChairs[i].SetActive(false);
        }
    }
    public List<PlayerController> OnPlayerChairs(int playerCount)
    {
        List<PlayerController> newPlayers=new List<PlayerController>();
        for (int i = 0; i < playerCount; i++)
        {
            playerChairs[i].SetActive(true);
            newPlayers.Add(playerChairs[i].GetComponent<PlayerController>());
        }
        return newPlayers;
    }
}
