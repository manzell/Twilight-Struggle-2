using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 

public class Player : MonoBehaviour
{
    public Faction faction;

    public int victoryPoints = 0;
    public int milOps = 0;
    public List<Card> hand = new();

    public Player enemyPlayer => Game.Players.First(player => player != this);

    public void AdjustVP(int amount)
    {
        victoryPoints += amount;
        Game.AdjustVP(this, amount); 
    }

    public void AdjustMilOps(int amount)
    {
        milOps = Mathf.Clamp(milOps + amount, 0, 5);
        Game.AdjustMilOps(this, amount); 
    }

    public void Discard(Card card)
    {
        if (hand.Remove(card))
            Game.discards.Add(card); 
    }
}
