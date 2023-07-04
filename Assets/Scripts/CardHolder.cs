using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardHolder : MonoBehaviour
{
    public List<Card> cards;

    public TextMeshProUGUI cardCountText;
    private void Start()
    {
        cardCountText.text = cards.Count.ToString();
    }
}
