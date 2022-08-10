using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System; 
using System.Linq;
using System.Threading.Tasks;

public class Realign : PlayerAction
{
    public static event Action<RealignAttempt> prepareRealign, realignEvent;

    public List<RealignAttempt> Attempts => attempts;  
    List<RealignAttempt> attempts = new();

    IEnumerable<Country> EligibleCountries(Player player) => Game.Countries.Where(c => c.Can(this) && c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(Player.enemyPlayer) > 0);

    public override bool Can(Player player, Card card) => card.Data is not ScoringCard && EligibleCountries(player).Count() > 0;

    protected override async Task Action()
    {
        SelectionManager<Country> selectionManager = new (EligibleCountries(Player));

        while (selectionManager.open && selectionManager.Selected.Count() < modifiedOpsValue)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select Realign Target ({modifiedOpsValue} remaining)");
            Attempt(await selectionManager.Selection);
        }

        selectionManager.Close();
    }

    async void Attempt(Country country)
    {
        int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

        RealignAttempt attempt = new(Player, country);

        prepareRealign?.Invoke(attempt);
        attempts.Add(attempt);

        int totalRoll = attempt.friendlyRoll.Value - attempt.enemyRoll.Value;

        Debug.Log($"{Player.name} Realignment Attempt vs {country.name}. {Player.name} Roll: {attempt.friendlyRoll.Value} vs. " +
            $"{Player.enemyPlayer.name} Roll: {attempt.enemyRoll.Value}");

        await attempt.realignCompletion.Task; 

        if (totalRoll > 0)
            country.AdjustInfluence(Player.enemyPlayer.faction, -totalRoll);
        if (totalRoll < 0)
            country.AdjustInfluence(Player.faction, totalRoll);

        realignEvent?.Invoke(attempt);
    }

    public class RealignAttempt
    {
        public TaskCompletionSource<RealignAttempt> realignCompletion;
        public Player player;
        public Country country;
        public Roll friendlyRoll = new Roll(0);
        public Roll enemyRoll = new Roll(0); 
        public int friendlyMod, enemyMod;

        public int TotalRoll => (friendlyRoll.Value + friendlyMod) - (enemyRoll.Value + enemyMod);
        public bool successful => TotalRoll > 0; 

        public RealignAttempt(Player player, Country country)
        {
            this.player = player;
            this.country = country;

            friendlyMod = country.Neighbors.Count(cd => cd.country.Control == player.faction) + (country.Control == player.faction ? 1 : 0);
            enemyMod = country.Neighbors.Count(cd => cd.country.Control == player.faction.enemyFaction) + (country.Control == player.enemyPlayer.faction ? 1 : 0);

            realignCompletion = new(); 
        }
    }
}
