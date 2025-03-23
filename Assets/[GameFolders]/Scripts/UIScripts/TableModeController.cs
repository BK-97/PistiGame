using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public enum TableLevels { Caylak,Kalfa,Usta}
public class TableModeController : MonoBehaviour
{
    private int maxBet;
    private int minBet;
    TableLevels tableLevel;

    [SerializeField] TextMeshProUGUI betTMP;
    [SerializeField] TextMeshProUGUI tableNameTMP;

    [SerializeField] GameObject blockerPanel;
    public void TableSet(RoomData roomData)
    {
        betTMP.text = "Bahis Aralýðý: "+ roomData.minBetValue.ToString() + "-" + roomData.maxBetValue.ToString();
        tableNameTMP.text = roomData.roomType.ToString();
        if (ExchangeManager.Instance.GetCurrency(CurrencyType.Cash) < minBet)
        {
            blockerPanel.SetActive(true);
        }
        else
        {
            blockerPanel.SetActive(false);
        }
    }
   
    public void CreateTablePanel()
    {
        TablesController.OnCreateTablePanel.Invoke(tableLevel);
    }
    public void OpenTables()
    {
        TablesController.OnFilterTablePanel.Invoke(tableLevel);
    }
}
