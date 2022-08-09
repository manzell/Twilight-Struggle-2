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

    public bool Can(Player player, Country country) => country.Influence(player.enemyPlayer) > 0;

    protected override async Task Action()
    {
        SelectionManager<Country> selectionManager = new (Game.Countries.Where(country => country.Influence(Player.enemyPlayer) > 0 && Game.DEFCON >= country.Continents.Max(c => c.defconRestriction)));

        while (selectionManager.open && selectionManager.Selected.Count() < modifiedOpsValue)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select Realign Target ({modifiedOpsValue} remaining)");
            Attempt(await selectionManager.Selection);
        }

        selectionManager.Close();
    }

    void Attempt(Country country)
    {
        int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

        RealignAttempt attempt = new(Player, country);

        prepareRealign?.Invoke(attempt);
        attempts.Add(attempt);

        int totalRoll = attempt.friendlyRoll.Value - attempt.enemyRoll.Value;

        Debug.Log($"{Player.name} Realignment Attempt vs {country.name}. {Player.name} Roll: {attempt.friendlyRoll.Value} vs. " +
            $"{Player.enemyPlayer.name} Roll: {attempt.enemyRoll.Value}");

        if (totalRoll > 0)
            country.AdjustInfluence(Player.enemyPlayer.faction, -totalRoll);
        if (totalRoll < 0)
            country.AdjustInfluence(Player.faction, totalRoll);

        realignEvent?.Invoke(attempt);
    }

    public class RealignAttempt
    {
        public Player player;
        public Country country;
        public Roll friendlyRoll, enemyRoll;

        public RealignAttempt(Player player, Country country)
        {
            this.player = player;
            this.country = country;
            
            friendlyRoll = new Roll(country.Neighbors.Count(cd => cd.country.Control == player.faction) + (country.Control == player.faction ? 1 : 0));
            enemyRoll = new Roll(country.Neighbors.Count(cd => cd.country.Control == player.faction.enemyFaction) + (country.Control == player.enemyPlayer.faction ? 1 : 0));            
        }
    }
}
