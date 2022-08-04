using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Card
{
    private CardData data;
    public UI_Card ui;
    public int ops; 

    public string cardText => data.cardText;
    public string name => data.name;
    public CardData Data => data;
    public Faction Faction => data.faction; 

    public virtual async Task Headline(Player player) => await DoContextEvents(player, data.headlineEffects.Count > 0 ? data.headlineEffects : data.playEffects);
    public virtual async Task Event(Player player) => await DoContextEvents(player, data.playEffects);

    public Card(CardData d)
    {
        data = d; 
        ops = data.opsValue; 
    }

    async Task DoContextEvents(Player player, List<CardEffect> contextEvents)
    {
        Debug.Log($"Doing Context Events for {player} on {this.name}");
        foreach (CardEffect contextEvent in contextEvents)
            await contextEvent.Event(this, player);
    }
}
