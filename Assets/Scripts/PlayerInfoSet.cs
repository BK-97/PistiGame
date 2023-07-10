using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInfoSet : MonoBehaviour
{
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    private void OnEnable()
    {
        GameManager.OnPlayerWin.AddListener(UpdateText);
        GameManager.OnPlayerLose.AddListener(UpdateText);
    }
    private void OnDisable()
    {
        GameManager.OnPlayerWin.RemoveListener(UpdateText);
        GameManager.OnPlayerLose.RemoveListener(UpdateText);
    }
    private void Start()
    {
        UpdateText(true);
    }
    private void UpdateText(bool status)
    {
        cashText.text = PlayerPrefs.GetInt(PrefsKeys.Cash, 1000).ToString();
        if (winText != null)
            winText.text = PlayerPrefs.GetInt(PrefsKeys.WinCount, 0).ToString();
        if (loseText != null)
            loseText.text = PlayerPrefs.GetInt(PrefsKeys.LoseCount, 0).ToString();
    }

}
