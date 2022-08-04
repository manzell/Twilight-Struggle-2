using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System; 
using System.Linq;
using System.Threading.Tasks;

[CreateAssetMenu]
public class Realign : GameAction, ITargetCountries
{
    public static event Action<RealignAttempt> prepareRealign, realignEvent;

    List<RealignAttempt> attempts;
    int realignOps;

    public List<Country> TargetCountries => attempts.Select(attempt => attempt.country).ToList(); 

    public bool Can(Player player, Country country) => country.Influence(player.enemyPlayer) > 0; 

    public override async Task Event(Player player, Card card)
    {
        this.player = player;
        this.card = card;
        realignOps = card.ops;

        selectionManager = new CountrySelectionManager(Game.countries.Where(country => country.Influence(player.enemyPlayer) > 0 && Game.DEFCON >= country.Continents.Max(c => c.defconRestriction)),
            Realign);

        while (realignOps > 0)
            await Task.Yield();

        selectionManager.CloseSelectionManager(); 

        void Realign(Country country)
        {
            int modifier = Game.currentPhase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0); 
            
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
            
            friendlyRoll = new Roll(country.Neighbors.Count(cd => cd.country.control == player.faction) + (country.control == player.faction ? 1 : 0));
            enemyRoll = new Roll(country.Neighbors.Count(cd => cd.country.control == player.faction.enemyFaction) + (country.control == player.enemyPlayer.faction ? 1 : 0));            
        }
    }
}
