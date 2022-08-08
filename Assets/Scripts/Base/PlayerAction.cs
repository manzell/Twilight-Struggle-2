using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

public abstract class PlayerAction : ISelectable
{
    Card card;
    Player player;
    public Card Card => card;
    public Player Player => player;
    public int modifier => Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

    public List<Effect> requiredEffects = new();
    public List<Effect> probitedEffects = new();

    UI_ActionSelection ui => GameObject.FindObjectOfType<UI_ActionSelection>();

    public event Action<ISelectable> onClick
    {
        add { ui.onClickHandler += value; }
        remove { ui.onClickHandler -= value; }
    }

    protected abstract Task Action(Player player, Card card);

    public virtual bool Can(Player player, Card card)
    {
        Debug.Log($"Can: REQ {requiredEffects.Count} ALL? {requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect))}" +
            $"PROH {probitedEffects.Count} ANY ? {probitedEffects.All(prohibitedEffects => !Game.activeEffects.Contains(prohibitedEffects))}");

        return requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect)) &&
            probitedEffects.All(prohibitedEffects => !Game.activeEffects.Contains(prohibitedEffects));
    }

    public void SetCard(Card card) => this.card = card;
    public void SetPlayer(Player player) => this.player = player;

    public Task Event(Player player) => Event(player, card);
    public Task Event(Card card) => Event(player, card);
    public Task Event(Player player, Card card)
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{player.name} triggers {card.name}");
        this.player = player;
        this.card = card;
        return Action(player, card); 
    }

    public void OnSelectable()
    {
        ui.Summon(this); 
    }

    public void RemoveSelectable()
    {
        // We tell our UI to remove our drawing
    }
}
