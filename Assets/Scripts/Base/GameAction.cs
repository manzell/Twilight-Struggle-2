using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public abstract class GameAction : ScriptableObject
{
    public Card card;
    public Player player;

    public int modifier => Game.currentPhase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);
    protected CountrySelectionManager selectionManager;

    public virtual bool Can(Player player, Card card) => true;
    public abstract Task Event(Player player, Card card); 
}
