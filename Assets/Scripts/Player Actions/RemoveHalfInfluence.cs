using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RemoveHalfInfluence : PlayerAction
{
    enum RoundDirection { Up, Down }

    [SerializeField] CountryData countryData;
    [SerializeField] RoundDirection roundDirection = RoundDirection.Down;

    public override Task Action()
    {
        Country country = countryData.country; 

        int amtToRemove = roundDirection == RoundDirection.Up ? -Mathf.CeilToInt(country.Influence(Player.Faction) / 2f) : -Mathf.FloorToInt(country.Influence(Player.Faction) / 2f);

        if (amtToRemove != 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Removing half (Rounded {roundDirection}) {Player.Faction.name} Influence in {countryData.name}");
            country.AdjustInfluence(Player.Faction, -amtToRemove);
        }

        return Task.CompletedTask; 
    }
}
