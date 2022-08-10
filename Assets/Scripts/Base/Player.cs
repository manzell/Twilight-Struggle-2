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

    private void Awake()
    {
        faction.player = this; 
    }

    public void AdjustVP(int amount)
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{(amount > 0 ? name : enemyPlayer.name)} Scores {Mathf.Abs(amount)} VPs");
        victoryPoints += amount;
        Game.AdjustVPEvent(this, amount);
    }

    public void AdjustMilOps(int amount)
    {
        amount = Mathf.Clamp(milOps + amount, 0, 5) - milOps;
        Game.AdjustMilOpEvent(this, amount);
    }

    public void Discard(Card card)
    {
        if (hand.Remove(card))
            Game.discards.Add(card); 
    }
}
