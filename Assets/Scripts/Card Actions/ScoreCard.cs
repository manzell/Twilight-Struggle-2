using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using System.Threading.Tasks;

public class ScoreCard : PlayerAction
{
    public static event Action<ScoreCard> prepareScoring, scoringEvent;
    [SerializeField] Continent continent;
    [SerializeField] Dictionary<Continent.ControlStatus, int> scoringTable = new();
    public TaskCompletionSource<ScoreCard> scoringTask;

    public Continent Continent => continent;
    public Dictionary<Continent.ControlStatus, int> ScoringTable => scoringTable;

    public Dictionary<Player, int> Battlegrounds => new Dictionary<Player, int> {
        {Player, continent.countries.Count(country => country.Battleground && country.Control == Player.Faction) },
        {Player.Enemy, continent.countries.Count(country => country.Battleground && country.Control == Player.Enemy.Faction) }
    };

    public Dictionary<Player, int> AdjacentSuperpowers => new Dictionary<Player, int> {
        {Player, continent.countries.Count(country => country.AdjacentSuperpower == Player.Enemy && country.Control == Player.Faction) },
        {Player.Enemy, continent.countries.Count(country => country.AdjacentSuperpower == Player.Enemy && country.Control == Player.Enemy.Faction) }
    };

    public Dictionary<Player, int> CountryCount => new Dictionary<Player, int> {
        {Player, continent.countries.Count(country => country.Control == Player.Faction) },
        {Player.Enemy, continent.countries.Count(country => country.Control == Player.Enemy.Faction) }
    };

    protected override Task Action()
    {
        scoringTask = new(); 
        prepareScoring?.Invoke(this);
        scoringEvent?.Invoke(this); 
        Debug.Log($"{Player.name} [{continent.GetControlStatus(Player)}] scores {continent.name} for {VP(Player) - VP(Player.Enemy)} {(Mathf.Abs(VP(Player) - VP(Player.Enemy)) == 1 ? "VP" : "VPs")} " +
            $"[{Player.Enemy.name} {continent.GetControlStatus(Player.Enemy)}]");
        Debug.Log($"{Player.name}: {Battlegrounds[Player]} Battlegrounds, {CountryCount[Player]} Countries, +{AdjacentSuperpowers[Player]} Adjacent to Superpower bonus");
        Debug.Log($"{Player.Enemy.name}: {Battlegrounds[Player.Enemy]} Battlegrounds, {CountryCount[Player.Enemy]} Countries, +{AdjacentSuperpowers[Player.Enemy]} Adjacent to Superpower bonus");

        Player.AdjustVP(VP(Player) - VP(Player.Enemy));
        return Task.CompletedTask; 
    }

    public int VP(Player player) => scoringTable[continent.GetControlStatus(player)] + Battlegrounds[player] + AdjacentSuperpowers[player];
}
