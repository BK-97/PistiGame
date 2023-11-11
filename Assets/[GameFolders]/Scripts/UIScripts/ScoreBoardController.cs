using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreBoardController : MonoBehaviour
{
    [SerializeField]
    private List<BoardMember> boardMembers=new List<BoardMember>();
    public GameObject boardMemberPrefab;
    public Transform boardMembersParent;
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
        SpawnBoardMembers();
        for (int i = 0; i < playerControllers.Count; i++)
        {
            boardMembers[i].SetInfo(i+1, playerControllers[i].playerType.ToString(), playerControllers[i].point);
        }
        ShowBoard();

        if (playerControllers[0].playerType == PlayerController.PlayerTypes.Player)
            GameManager.OnPlayerWin.Invoke(true);
        else
            GameManager.OnPlayerLose.Invoke(false);
    }
    public void ShowBoard()
    {
        for (int i = 0; i < GameManager.Instance.currentPlayers.Count; i++)
        {
            boardMembers[i].gameObject.SetActive(true);
        }
    }
    private void SpawnBoardMembers()
    {
        for (int i = 0; i < GameManager.Instance.currentPlayers.Count; i++)
        {
            var go=Instantiate(boardMemberPrefab, boardMembersParent);
            boardMembers.Add(go.GetComponent<BoardMember>());
        }
    }
}
