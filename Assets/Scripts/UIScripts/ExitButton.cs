using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public GameObject currentPanel;
    public GameObject nextPanel;
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ClosePanel()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
}
