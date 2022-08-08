using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class ScoreCard : PlayerAction
{
    [SerializeField] Continent continent;
    [SerializeField] Dictionary<Continent.ControlStatus, int> scoringTable = new();
    [SerializeField] Dictionary<CountryData, int> countryBonuses = new();

    protected override Task Action(Player player, Card card)
    {
        int vp1 = VP(player);
        int vp2 = VP(player.enemyPlayer);

        Debug.Log($"{player.name} [{continent.GetControlStatus(player)}] scores {continent.name} for {vp1 - vp2} {(Mathf.Abs(vp1 - vp2) == 1 ? "VP" : "VPs")} " +
            $"[{player.enemyPlayer.name} {continent.GetControlStatus(player.enemyPlayer)}]");

        player.AdjustVP(vp1 - vp2);

        return Task.CompletedTask; 
    }

    public int VP(Player player) =>
        scoringTable[continent.GetControlStatus(player)] +
        continent.countries.Count(country => country.Control == player.faction && country.Battleground) +
        continent.countries.Count(country => country.Control == player.faction && country.AdjacentSuperpower == player.enemyPlayer.faction) +
        countryBonuses.Sum(data => data.Key.country.Control == player.faction ? data.Value : 0);
}
