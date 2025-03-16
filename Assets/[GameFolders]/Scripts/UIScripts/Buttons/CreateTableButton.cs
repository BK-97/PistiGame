using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTableButton : MonoBehaviour
{
    private RoomModeController tableController;
    private void Start()
    {
        tableController = GetComponentInParent<RoomModeController>();
    }
    public void TriggerButton()
    {
        UIManager.Instance.CreateTablePanel(tableController.minBet,tableController.maxBet);
    }
}
