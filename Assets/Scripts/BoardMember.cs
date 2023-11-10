using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoardMember : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerIndex;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerScore;
    public void SetInfo(int index,string name,int score)
    {
        playerIndex.text = index.ToString() + ".";
        playerName.text = name;
        playerScore.text = score.ToString();
    }
}
