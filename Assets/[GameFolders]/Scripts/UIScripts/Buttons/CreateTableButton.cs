using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTableButton : MonoBehaviour
{
    private TableController tableController;
    private void Start()
    {
        tableController = GetComponentInParent<TableController>();
    }
    public void TriggerButton()
    {
        UIManager.Instance.CreateTablePanel(tableController.minBet,tableController.maxBet);
    }
}
