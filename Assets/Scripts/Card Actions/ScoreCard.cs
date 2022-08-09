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

    protected override Task Action()
    {
        int vp1 = VP(Player);
        int vp2 = VP(Player.enemyPlayer);

        Debug.Log($"{Player.name} [{continent.GetControlStatus(Player)}] scores {continent.name} for {vp1 - vp2} {(Mathf.Abs(vp1 - vp2) == 1 ? "VP" : "VPs")} " +
            $"[{Player.enemyPlayer.name} {continent.GetControlStatus(Player.enemyPlayer)}]");

        Player.AdjustVP(vp1 - vp2);

        return Task.CompletedTask; 
    }

    public int VP(Player player) =>
        scoringTable[continent.GetControlStatus(player)] +
        continent.countries.Count(country => country.Control == player.faction && country.Battleground) +
        continent.countries.Count(country => country.Control == player.faction && country.AdjacentSuperpower == player.enemyPlayer.faction) +
        countryBonuses.Sum(data => data.Key.country.Control == player.faction ? data.Value : 0);
}
