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

    public IEnumerable<Country> RealignedCountries => attempts.Select(attempt => attempt.country).Distinct();
    public List<Country> TargetCountries => attempts.Select(attempt => attempt.country).ToList();

    List<RealignAttempt> attempts;
    int realignOps;

    public bool Can(Player player, Country country) => country.Influence(player.enemyPlayer) > 0;

    protected override async Task Action(Player player, Card card)
    {
        realignOps = card.ops;

        SelectionManager<Country> selectionManager = new (Game.Countries.Where(country => country.Influence(player.enemyPlayer) > 0 && Game.DEFCON >= country.Continents.Max(c => c.defconRestriction)),
            Realign);

        while (selectionManager.open && realignOps + modifier > 0)
            await selectionManager.Selection;

        selectionManager.Close(); 

        void Realign(Country country)
        {
            int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0); 
            
            RealignAttempt attempt = new(player, country);

            prepareRealign(attempt); 
            attempts.Add(attempt); 
            
            int totalRoll = attempt.friendlyRoll.Value - attempt.enemyRoll.Value;

            if (totalRoll > 0)
                country.AdjustInfluence(player.enemyPlayer.faction, -totalRoll);
            if (totalRoll < 0)
                country.AdjustInfluence(player.faction, totalRoll);

            realignEvent(attempt);
            realignOps--;
        }
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
