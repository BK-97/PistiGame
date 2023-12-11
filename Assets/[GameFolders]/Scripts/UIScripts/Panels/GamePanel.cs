using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.ScoreBoardEvent.AddListener(HidePanel);
    }
    private void OnDisable()
    {
        GameManager.ScoreBoardEvent.RemoveListener(HidePanel);
    }
    private void HidePanel(List<PlayerController> playerControllers)
    {
        gameObject.SetActive(false);
    }
}
