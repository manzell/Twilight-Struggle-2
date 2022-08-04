using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Adjust VP")]
public class AdjustVP : CardEffect
{
    [SerializeField] Faction faction;
    [SerializeField] int vpAmount; 

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        (card.Faction == player.enemyPlayer.faction ? player.enemyPlayer : player).AdjustVP(vpAmount);

        return Task.CompletedTask; 
    }
}
