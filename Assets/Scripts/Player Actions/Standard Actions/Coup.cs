using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks; 

public class Coup : PlayerAction, IUseOps
{
    public static event Action<CoupAttempt> prepareCoupEvent, coupEvent;

    public List<CoupAttempt> Attempts => new() { attempt };

    public int OpsValue { get => ops; set => ops = value; }
    int ops;

    CoupAttempt attempt;

    IEnumerable<Country> EligibleCountries(Player player) => Game.Countries.Where(c => c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(player.Enemy) > 0 && c.Can(this));

    public override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{Player.name} Select Coup Target");
        SelectionManager<Country> selectionManager = new(EligibleCountries(Player));
        Country country = await selectionManager.Selection as Country;
        attempt = new(country, Player, OpsValue);

        selectionManager.Close();

        Debug.Log($"{OpsValue}-Op Coup vs. {attempt.country.name}. Roll: {attempt.roll.Value} [{attempt.country.Stability * 2}]");

        await attempt.coupCompletion.Task; 
            
        Player.milOps += OpsValue; 

        if (attempt.enemyInfluenceRemoved > 0)
            attempt.country.AdjustInfluence(Player.Enemy.Faction, -attempt.enemyInfluenceRemoved);
        if (attempt.influenceToAdd > 0)
            attempt.country.AdjustInfluence(Player.Faction, attempt.influenceToAdd);

        if (attempt.country.Battleground)
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
        public int enemyInfluenceRemoved, influenceToAdd;
        public bool successful => coupValue > 0;

        public CoupAttempt(Country country, Player player, int ops)
        {
            this.country = country;
            this.player = player;
            this.ops = ops;
            modifier = 0; // For cards that modify Coup Rolls, such as Latin American Death Squads, SALT, as well as events that trigger off of coups like CMC and Yuri + Samantha
            roll = new Roll(0);
            coupCompletion = new();

            enemyInfluenceRemoved = (int)MathF.Min(coupValue, country.Influence(player.Enemy));
            influenceToAdd = (int)MathF.Max(0, coupValue - enemyInfluenceRemoved);

            prepareCoupEvent?.Invoke(this); 
                coupEvent?.Invoke(this);
        }
    }
}