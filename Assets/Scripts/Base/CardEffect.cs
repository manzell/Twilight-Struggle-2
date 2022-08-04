using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

[System.Serializable]
public abstract class CardEffect
{
    public List<Effect> requiredEffects = new();
    public List<Effect> probitedEffects = new();

    public abstract Task Event(Card card, Player player);

    public virtual bool Can(Card card, Player player)
    {
        return requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect)) &&
            probitedEffects.Any(prohibitedEffects => Game.activeEffects.Contains(prohibitedEffects)); 
    }
}
