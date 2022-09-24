using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 
using System.Threading.Tasks;

public class TriggerCardEvent : PlayerAction
{
    Card card;
    public void SetCard(Card card) => this.card = card; 

    public override async Task Action() => await card.Event(Player);
}
