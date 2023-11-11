using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardCollector : MonoBehaviour
{
    public List<Card> collectedCards;
    private int point;
    public TextMeshProUGUI textMesh;
    bool calculate=true;
    private PlayerController playerController;
    public PlayerController PlayerController { get { return (playerController == null) ? playerController = GetComponentInParent<PlayerController>() : playerController; } }
    private void OnEnable()
    {
        GameManager.OnCalculateScore.AddListener(()=> calculate=false);
    }
    private void OnDisable()
    {
        GameManager.OnCalculateScore.RemoveListener(()=> calculate=false);

    }
    private void Update()
    {
        if (!calculate)
            return;
        if(collectedCards.Count!=transform.childCount)
            CalculatePoint();
    }
    public void HasMoreCard()
    {
        point += 3;
        CalculatePoint();
    }
    public void CalculatePoint()
    {
        collectedCards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            collectedCards.Add(transform.GetChild(i).gameObject.GetComponent<Card>());
        }
        int currentPoint=0;
        for (int i = 0; i < collectedCards.Count; i++)
        {
            currentPoint += collectedCards[i].GetPoint();
        }
        point = currentPoint;
        textMesh.text ="SCORE : "+ point.ToString();
        PlayerController.point = point;
    }
}
