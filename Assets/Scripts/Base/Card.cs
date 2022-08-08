using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq; 
using System.Threading.Tasks;

public class Card : ISelectable
{
    private CardData data;
    public UI_Card ui;
    public int ops;

    public string cardText => data.cardText;
    public string name => data.name;
    public CardData Data => data;
    public Faction Faction => data.faction;

    public event Action<ISelectable> onClick
    {
        add { ui.onClickHandler += value; }
        remove { ui.onClickHandler -= value; }
    }

    public virtual async Task Headline(Player player) => await DoContextEvents(player, data.headlineActions.Count > 0 ? data.headlineActions : data.playActions);
    public virtual async Task Event(Player player) => await DoContextEvents(player, data.playActions);

    public Card(CardData d)
    {
        data = ScriptableObject.Instantiate(d); 
        ops = data.opsValue; 
    }

    async Task DoContextEvents(Player player, List<PlayerAction> contextEvents)
    {
        foreach (PlayerAction contextEvent in contextEvents)
            await contextEvent.Event(player, this);
    }

    public bool CanHeadline(Player player) => data.headlineActions.Count == 0 || data.headlineActions.All(effect => effect.Can(player, this));
    public bool CanEvent(Player player) => data.playActions.Count == 0 || data.playActions.All(effect => effect.Can(player, this));

    public void OnSelectable() => ui.SetHighlight(Color.yellow);
    public void RemoveSelectable() => ui.ClearHighlight(); 
}
