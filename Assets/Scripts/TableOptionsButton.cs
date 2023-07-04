using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableOptionsButton : MonoBehaviour
{
    public GameObject tableOptions;
    public void OpenTableOptions()
    {
        tableOptions.SetActive(true);
    }
}
