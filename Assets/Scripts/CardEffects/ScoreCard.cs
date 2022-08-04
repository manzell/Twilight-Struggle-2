using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName ="Card/Scoring Card")]
public class ScoreCard : CardEffect
{
    [SerializeField] Continent continent;
    [SerializeField] Dictionary<Continent.ControlStatus, int> scoringTable = new Dictionary<Continent.ControlStatus, int>();
    [SerializeField] Dictionary<Country, int> countryBonuses = new Dictionary<Country, int>();

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event for {card.name} received");
        player.AdjustVP(VP(player) - VP(player.enemyPlayer));
        Game.discards.Add(card); 
        Game.currentPhase.Continue();

        return Task.CompletedTask; 
    }

    public int VP(Player player) =>
        Game.countries.Count(country => country.Continents.Contains(continent) && country.control == player.faction && country.AdjacentSuperower == player.enemyPlayer.faction) +
        Game.countries.Count(country => country.Continents.Contains(continent) && country.control == player.faction && country.Battleground) +
        countryBonuses.Keys.Sum(country => country.control == player.faction ? countryBonuses[country] : 0) +
        scoringTable[continent.GetControlStatus(player)];
}
