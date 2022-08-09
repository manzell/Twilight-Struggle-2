using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks; 

public class Coup : PlayerAction
{
    public static event Action<Coup> prepareCoupEvent, coupEvent;

    public List<CoupAttempt> Attempts => attempts; 
    List<CoupAttempt> attempts = new(); 

    protected override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{Player.name} Select Coup Target");
        SelectionManager<Country> selectionManager = new(Game.Countries.Where(c => c.Can(this) && c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(Player.enemyPlayer) > 0));

        Country country = await selectionManager.Selection;
        selectionManager.Close();

        CoupAttempt attempt = new(country, Player, modifiedOpsValue);
        attempts.Add(attempt); 
            
        int enemyInfluenceToRemove = (int)MathF.Min(attempt.coupValue, country.Influence(Player.enemyPlayer));
        int influenceToAdd = (int)MathF.Max(0, attempt.coupValue - enemyInfluenceToRemove);

        prepareCoupEvent?.Invoke(this);
            
        Player.milOps += modifiedOpsValue; 

        if (enemyInfluenceToRemove > 0)
            country.AdjustInfluence(Player.enemyPlayer.faction, -enemyInfluenceToRemove);
        if (influenceToAdd > 0)
            country.AdjustInfluence(Player.faction, influenceToAdd);

        coupEvent?.Invoke(this);

        twilightStruggle.UI.UI_Message.SetMessage($"{Card.ops}-Op ({(modifiedOpsValue >= 0 ? "+" + modifiedOpsValue : modifiedOpsValue)}) + Roll: {attempt.roll.Value} Coup vs {country.name} [{country.Stability * 2}]");

        if (country.Battleground)
            Game.AdjustDEFCON(-1); // Todo: Move this to a "Game Rule" that just listens for things like Nuke Subs. 
    }

    public class CoupAttempt
    {
        public int ops { get; private set; }
        public int modifier { get; private set; }
        public Roll roll { get; private set; }
        public Player player { get; private set; }
        public Country country { get; private set; }

        public bool successful => coupValue > 0;
        public int coupValue => roll.Value + ops - coupDefense;
        public int coupDefense => country.Stability * 2;

        public CoupAttempt(Country country, Player player, int ops)
        {
            this.country = country;
            this.player = player;
            this.ops = ops;
            modifier = 0; // For cards that modify Coup Rolls, such as Latin American Death Squads, SALT, as well as events that trigger off of coups like CMC and Yuri + Samantha
            roll = new Roll(0);
        }
    }
}