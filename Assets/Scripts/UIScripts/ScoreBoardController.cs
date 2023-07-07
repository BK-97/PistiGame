using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreBoardController : MonoBehaviour
{
    public List<GameObject> boardMembers;
    public List<TextMeshProUGUI> boardNames;
    public List<TextMeshProUGUI> scores;
    private void OnEnable()
    {
        GameManager.ScoreBoardEvent.AddListener(SetBoard);
    }
    private void OnDisable()
    {
        GameManager.ScoreBoardEvent.RemoveListener(SetBoard);
    }

    public void SetBoard(List<PlayerController> playerControllers)
    {
        Debug.Log(playerControllers[0].name);
        ShowBoard();
        for (int i = 0; i < playerControllers.Count; i++)
        {
            boardNames[i].text =playerControllers[i].playerType.ToString();
            scores[i].text =playerControllers[i].point.ToString();
        }
    }
    public void ShowBoard()
    {
        for (int i = 0; i < GameManager.Instance.currentPlayerCount; i++)
        {
            boardMembers[i].SetActive(true);
        }
    }
}
