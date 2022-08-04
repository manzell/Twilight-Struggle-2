using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks; 

[CreateAssetMenu]
public class Coup : GameAction, ITargetCountry
{
    public static event Action<Coup> prepareCoupEvent, coupEvent;

    public Country TargetCountry => targetCountry;
    [SerializeField] Country targetCountry;

    public override async Task Event(Player player, Card card)
    {
        this.player = player; 
        this.card = card;

        selectionManager = new CountrySelectionManager(Game.countries.Where(c => c.Can(this) && c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(player.enemyPlayer) > 0), 
           Coup);

        while (selectionManager.selectedCountries.Count == 0)
            await Task.Yield();

        selectionManager.CloseSelectionManager(); 

        void Coup(Country country)
        {
            CoupAttempt attempt = new(country, player, card); 
            
            int enemyInfluenceToRemove = (int)MathF.Min(attempt.coupValue, country.Influence(player.enemyPlayer));
            int influenceToAdd = (int)MathF.Max(0, attempt.coupValue - enemyInfluenceToRemove);

            prepareCoupEvent.Invoke(this);
            
            player.milOps += modifier + card.ops; 

            if (enemyInfluenceToRemove > 0)
                country.AdjustInfluence(player.enemyPlayer.faction, -enemyInfluenceToRemove);
            if (influenceToAdd > 0)
                country.AdjustInfluence(player.faction, influenceToAdd);

            coupEvent.Invoke(this);

            if (country.Battleground)
                Game.AdjustDEFCON(-1); // Todo: Move this to a "Game Rule" that just listens for things like Nuke Subs. 
        }
    }

    public class CoupAttempt
    {
        Country country;
        Player player; 
        Card card;
        int ops, modifier, coupDefense;
        Roll roll;

        public bool successful => coupValue > 0;
        public int coupValue => roll.Value + card.ops - coupDefense; 

        public CoupAttempt(Country country, Player player, Card card)
        {
            this.country = country;
            this.player = player;
            this.card = card; 
            coupDefense = country.Stability * 2;
            roll = new Roll(modifier); 
        }
    }
}

public interface IOpsValue
{
    public int opsAdjustment { get; }
}

public interface ITargetCountry
{
    public Country TargetCountry { get; }
}

public interface ITargetCountries
{
    public List<Country> TargetCountries { get; }
}
