using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerTypes { Player,Bot1,Bot2,Bot3}
    public PlayerTypes playerType;
    public List<Card> currentDeck;
    public Transform deckParent;
    public void CardAdd(Card addThis)
    {
        currentDeck.Add(addThis);
    }
    public void RemoveCard(Card removeThis)
    {
        currentDeck.Remove(removeThis);
    }

    public void CanPlay()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            currentDeck[i].canBeUse = true;
        }
    }
    public void CantPlay()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            currentDeck[i].canBeUse = false;
        }
    }
}
