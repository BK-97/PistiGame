using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TablesController : MonoBehaviour
{
    #region Params
    public RoomDataConfig roomDatas;
    public List<TableModeController> Tables;
    public TableCreator tableCreator;
    public TableLister tableLister;
    #endregion
    #region Events
    public static TableEvent OnCreateTablePanel = new TableEvent();
    public static TableEvent OnFilterTablePanel = new TableEvent();
    #endregion
    #region Mono
    private void Start()
    {
        for (int i = 0; i < Tables.Count; i++)
        {
            Tables[i].TableSet(roomDatas.roomDatas[i]);
        }
    }
    private void OnEnable()
    {
        OnCreateTablePanel.AddListener(OpenTableCreator);
        OnFilterTablePanel.AddListener(OpenListTablePanel);
    }
    private void OnDisable()
    {
        OnCreateTablePanel.RemoveListener(OpenTableCreator);
        OnFilterTablePanel.RemoveListener(OpenListTablePanel);
    }
    #endregion
    #region MainMethods
    private void OpenTableCreator(TableLevels tableLevel)
    {
        tableCreator.gameObject.SetActive(true);
        foreach (var item in roomDatas.roomDatas)
        {
            if (item.roomType == tableLevel)
            {
                tableCreator.Initialize(item);
            }
        }
    }
    private void OpenListTablePanel(TableLevels tableLevel)
    {
        tableLister.gameObject.SetActive(true);
        foreach (var item in roomDatas.roomDatas)
        {
            if (item.roomType == tableLevel)
            {
                tableLister.Initialize(item);
            }
        }
    }
    #endregion
}
public class TableEvent : UnityEvent<TableLevels> { }