using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System; 
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class Realign : PlayerAction, IUseOps
    {
        public static event Action<RealignAttempt> prepareRealign, realignEvent;

        public List<RealignAttempt> Attempts => attempts;

        public int OpsValue { get => ops; set => ops = value; }
        int ops;

        List<RealignAttempt> attempts;

        IEnumerable<Country> EligibleCountries(Player player) => Game.Countries.Where(c => c.Can(this) && c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(player.Enemy) > 0);

        public override bool Can(Player player, Card card) => card.Data is not ScoringCard && EligibleCountries(player).Count() > 0;

        public override async Task Action()
        {
            attempts = new();
            SelectionManager<Country> selectionManager = new(EligibleCountries(Player));

            while (selectionManager.open && selectionManager.Selected.Count() < OpsValue)
            {
                TwilightStruggle.UI.Message.SetMessage($"Select Realign Target ({OpsValue - attempts.Count()} remaining)");
                Country country = await selectionManager.Selection as Country;

                RealignAttempt attempt = new(Player, country);
                int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);
                int totalRoll = attempt.friendlyRoll.Value - attempt.enemyRoll.Value;

                attempts.Add(attempt);

                Debug.Log($"{Player.name} Realignment Attempt vs {country.name}. {Player.name} Roll: {attempt.friendlyRoll.Value} vs. " +
                    $"{Player.Enemy.name} Roll: {attempt.enemyRoll.Value}");

                await attempt.realignCompletion.Task;

                if (totalRoll > 0)
                    country.AdjustInfluence(Player.Enemy.Faction, -totalRoll);
                if (totalRoll < 0)
                    country.AdjustInfluence(Player.Faction, totalRoll);
            }

            selectionManager.Close();
        }

        public class RealignAttempt
        {
            public TaskCompletionSource<RealignAttempt> realignCompletion;
            public Player player;
            public Country country;
            public Roll friendlyRoll = new Roll(0);
            public Roll enemyRoll = new Roll(0);
            public int friendlyMod, enemyMod;

            public int influenceToRemove => Mathf.Min(Mathf.Abs(roll), country.Influence(losingFaction));
            public Faction winningFaction => roll >= 0 ? player.Faction : player.Enemy.Faction;
            public Faction losingFaction => roll < 0 ? player.Faction : player.Enemy.Faction;

            int roll => (friendlyRoll.Value + friendlyMod) - (enemyRoll.Value + enemyMod);

            public RealignAttempt(Player player, Country country)
            {
                prepareRealign?.Invoke(this);
                this.player = player;
                this.country = country;

                foreach (CountryData c in country.Neighbors)
                {
                    if (c.country.Control == player.Faction)
                        Debug.Log($"{c.name} is {player.name} Controlled; +1 to their Roll");
                }

                friendlyMod = country.Neighbors.Count(cd => cd.country.Control == player.Faction) + (country.Control == player.Faction ? 1 : 0);
                enemyMod = country.Neighbors.Count(cd => cd.country.Control == player.Faction.enemyFaction) + (country.Control == player.Enemy.Faction ? 1 : 0);

                realignEvent?.Invoke(this);
                realignCompletion = new();
            }
        }
    }
}