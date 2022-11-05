using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class ScoreCard : PlayerAction
    {
        public static event Action<ScoringAttempt> prepareScoring, scoringEvent;
        [SerializeField] Continent continent;
        [SerializeField] Dictionary<Continent.ControlStatus, int> scoringTable = new();
        public TaskCompletionSource<ScoringAttempt> scoringTask;

        public Continent Continent => continent;
        public Dictionary<Continent.ControlStatus, int> ScoringTable => scoringTable;

        public override async Task Action()
        {
            scoringTask = new();

            ScoringAttempt attempt = new(continent, Player, scoringTable);

            Debug.Log($"{Player.name} [{continent.GetControlStatus(Player, attempt)}] scores {continent.name} for " +
                $"{attempt.VP(Player) - attempt.VP(Player.Enemy)} {(Mathf.Abs(attempt.VP(Player) - attempt.VP(Player.Enemy)) == 1 ? "VP" : "VPs")} " +
                $"[{Player.Enemy.name} {continent.GetControlStatus(Player.Enemy, attempt)}]");

            Player.AdjustVP(attempt.VP(Player) - attempt.VP(Player.Enemy));
            scoringEvent?.Invoke(attempt);

            await attempt.scoringTask.Task;
        }

        public class ScoringAttempt
        {
            Player player;
            Continent continent;
            Dictionary<Continent.ControlStatus, int> scoringTable;
            public Dictionary<Player, IEnumerable<Country>> battlegrounds, adjacentSuperpowers, countryCount;
            public TaskCompletionSource<ScoringAttempt> scoringTask;

            public Player Player => player;
            public Continent Continent => continent;
            public int VP(Player player) => scoringTable[continent.GetControlStatus(player, this)] + battlegrounds[player].Count() + adjacentSuperpowers[player].Count();

            public Dictionary<Player, int> CountryCount => countryCount.ToDictionary(player => player.Key, player => player.Value.Count());
            public Dictionary<Player, int> Battlegrounds => battlegrounds.ToDictionary(player => player.Key, player => player.Value.Count());
            public Dictionary<Player, int> AdjacentSuperpowers => adjacentSuperpowers.ToDictionary(player => player.Key, player => player.Value.Count());
            public Continent.ControlStatus ControlStatus(Player player) => continent.GetControlStatus(player, this);

            public ScoringAttempt(Continent continent, Player player, Dictionary<Continent.ControlStatus, int> scoringTable)
            {
                this.player = player;
                this.scoringTable = scoringTable;
                this.continent = continent;
                scoringTask = new();
                battlegrounds = new Dictionary<Player, IEnumerable<Country>>
            {
                { player, continent.countries.Where(country => country.IsBattleground && country.Control == player.Faction) },
                { player.Enemy, continent.countries.Where(country => country.IsBattleground && country.Control == player.Enemy.Faction) }
            };

                adjacentSuperpowers = new Dictionary<Player, IEnumerable<Country>> {
                {player, continent.countries.Where(country => country.AdjacentSuperpower == player.Enemy && country.Control == player.Faction) },
                {player.Enemy, continent.countries.Where(country => country.AdjacentSuperpower == player.Enemy && country.Control == player.Enemy.Faction) }
            };

                countryCount = new Dictionary<Player, IEnumerable<Country>> {
                {player, continent.countries.Where(country => country.Control == player.Faction) },
                {player.Enemy, continent.countries.Where(country => country.Control == player.Enemy.Faction) }
            };

                prepareScoring?.Invoke(this);
            }

            public void Close() => scoringTask.SetResult(this);
        }
    }
}