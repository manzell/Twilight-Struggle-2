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
    UI_ActionSelection ui;
    protected PlayerAction previousAction;

    public List<Effect> requiredEffects = new();
    public List<Effect> prohibitedEffects = new();

    public Card Card => card;
    public Player Player => player;
    public int modifiedOpsValue => card.ops + Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

    protected abstract Task Action();

    public virtual bool Can(Player player, Card card)
    {
        Debug.Log($"Can: REQ {requiredEffects.Count} ALL? {requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect))}" +
            $"PROH {prohibitedEffects.Count} ANY ? {prohibitedEffects.All(prohibitedEffects => !Game.activeEffects.Contains(prohibitedEffects))}");

        return requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect)) &&
            prohibitedEffects.All(prohibitedEffects => !Game.activeEffects.Contains(prohibitedEffects));
    }

    public virtual Task Event() => Event(player, card);
    public virtual Task Event(PlayerAction previousAction)
    {
        this.previousAction = previousAction; 
        return Event(previousAction.Player, previousAction.card);
    }
    public virtual Task Event(Player player) => Event(player, card);
    public virtual Task Event(Card card) => Event(player, card);
    public virtual Task Event(Player player, Card card)
    {
        this.player = player;
        this.card = card;

        // Do we check and modify the "active player" and the card ops value here? 

        return Action();
    }

    public virtual PlayerAction Clone() => (PlayerAction)this.MemberwiseClone();

    public void SetCard(Card card) => this.card = card;
    public void SetPlayer(Player player) => this.player = player;

    public event Action<ISelectable> onClick
    {
        add { ui.activationHandler += value; }
        remove { ui.activationHandler -= value; }
    }

    public void OnSelectable() => (ui = GameObject.FindObjectOfType<UI_ActionSelection>()).Summon(player, this);
    public void RemoveSelectable() => ui.Dismiss(this); 
}
