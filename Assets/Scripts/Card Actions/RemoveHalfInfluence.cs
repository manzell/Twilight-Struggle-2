using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RemoveHalfInfluence : PlayerAction
{
    enum RoundDirection { Up, Down }

    [SerializeField] CountryData countryData;
    [SerializeField] Faction faction; 
    [SerializeField] RoundDirection roundDirection = RoundDirection.Down;

    protected override Task Action(Player player, Card card)
    {
        Country country = countryData.country; 

        if (faction == null)
            faction = card.Faction ?? player.faction;

        int amtToRemove = roundDirection == RoundDirection.Up ? -Mathf.CeilToInt(country.Influence(faction) / 2f) : -Mathf.FloorToInt(country.Influence(faction) / 2f);

        if (amtToRemove != 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Removing half (Rounded {roundDirection}) {faction.name} Influence in {countryData.name}");
            country.AdjustInfluence(player.enemyPlayer.faction, -amtToRemove);
        }

        return Task.CompletedTask; 
    }
}
