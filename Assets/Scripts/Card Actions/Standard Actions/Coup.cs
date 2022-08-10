using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks; 

public class Coup : PlayerAction
{
    public static event Action<CoupAttempt> prepareCoupEvent, coupEvent;

    public List<CoupAttempt> Attempts => new() { attempt }; 
    CoupAttempt attempt;

    IEnumerable<Country> EligibleCountries(Player player) => Game.Countries.Where(c => c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(player.enemyPlayer) > 0 && c.Can(this));

    protected override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{Player.name} Select Coup Target");
        SelectionManager<Country> selectionManager = new(EligibleCountries(Player));

        Country country = await selectionManager.Selection;
        selectionManager.Close();

        attempt = new(country, Player, modifiedOpsValue);

        Debug.Log($"{Card.ops}-Op ({(modifiedOpsValue - Card.ops >= 0 ? "+" : string.Empty)}{modifiedOpsValue - Card.ops}) + " +
            $"Roll: {attempt.roll.Value} Coup vs {country.name} [{country.Stability * 2}]");

        prepareCoupEvent?.Invoke(attempt);

        await attempt.coupCompletion.Task; 
            
        Player.milOps += modifiedOpsValue; 

        if (attempt.enemyInfluenceRemoved > 0)
            country.AdjustInfluence(Player.enemyPlayer.faction, -attempt.enemyInfluenceRemoved);
        if (attempt.influenceToAdd > 0)
            country.AdjustInfluence(Player.faction, attempt.influenceToAdd);

        coupEvent?.Invoke(attempt);

        if (country.Battleground)
            Game.AdjustDEFCON(-1); // Todo: Move this to a "Game Rule" that just listens for things like Nuke Subs. 
    }

    public override bool Can(Player player, Card card) => card.Data is not ScoringCard && EligibleCountries(player).Count() > 0; 


    public class CoupAttempt
    {
        public TaskCompletionSource<CoupAttempt> coupCompletion; 
        public int ops { get; private set; }
        public int modifier { get; private set; }
        public Roll roll { get; private set; }
        public Player player { get; private set; }
        public Country country { get; private set; }

        public int coupDefense => country.Stability * 2;
        public int coupValue => roll.Value + ops - coupDefense;
        public int enemyInfluenceRemoved => (int)MathF.Min(coupValue, country.Influence(player.enemyPlayer));
        public int influenceToAdd => (int)MathF.Max(0, coupValue - enemyInfluenceRemoved);
        public bool successful => coupValue > 0;

        public CoupAttempt(Country country, Player player, int ops)
        {
            this.country = country;
            this.player = player;
            this.ops = ops;
            modifier = 0; // For cards that modify Coup Rolls, such as Latin American Death Squads, SALT, as well as events that trigger off of coups like CMC and Yuri + Samantha
            roll = new Roll(0);
            coupCompletion = new(); 
        }
    }
}