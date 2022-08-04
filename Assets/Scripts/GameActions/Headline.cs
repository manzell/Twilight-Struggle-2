using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[CreateAssetMenu]
public class Headline : GameAction
{
    public override async Task Event(Player player, Card card)
    {
        this.card = card;
        this.player = player;
        await Task.Yield(); 
    }

    public async Task Event(KeyValuePair<Player, Card> kvp)
    {
        this.card = kvp.Value;
        this.player = kvp.Key;
        await Task.Yield();
    }
}
