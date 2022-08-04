using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName ="CardEffect/RemoveHalfInfluence")]
public class RemoveHalfInfluence : CardEffect
{
    enum RoundDirection { Up, Down }

    [SerializeField] Country country;
    [SerializeField] Faction faction; 
    [SerializeField] RoundDirection roundDirection = RoundDirection.Down;

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event for {card.name} received");
        if (faction == null)
            faction = player.faction;

        float amtToRemove = country.Influence(faction) / 2f;

        country.AdjustInfluence(player.enemyPlayer.faction, roundDirection == RoundDirection.Up ? -Mathf.CeilToInt(amtToRemove) : -Mathf.FloorToInt(amtToRemove));

        return Task.CompletedTask; 
    }
}
