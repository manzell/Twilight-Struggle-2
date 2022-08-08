using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks; 

public class Coup : PlayerAction
{
    public static event Action<Coup> prepareCoupEvent, coupEvent;
    public List<CoupAttempt> attempts { get; private set; }

    protected override async Task Action(Player player, Card card)
    {
        twilightStruggle.UI.UI_Message.SetMessage($"{player.name} Select Coup Target");
        SelectionManager<Country> selectionManager = new(Game.Countries.Where(c => c.Can(this) && c.Continents.Max(c => c.defconRestriction) <= Game.DEFCON && c.Influence(player.enemyPlayer) > 0), 
           Coup);

        while(selectionManager.open && selectionManager.Selected.Count() == 0)
            await selectionManager.Selection;

        selectionManager.Close();

        void Coup(Country country)
        {
            Roll roll = new Roll(modifier); 
            CoupAttempt attempt = new(country, player, card, roll);
            attempts.Add(attempt); 
            
            int enemyInfluenceToRemove = (int)MathF.Min(attempt.coupValue, country.Influence(player.enemyPlayer));
            int influenceToAdd = (int)MathF.Max(0, attempt.coupValue - enemyInfluenceToRemove);

            prepareCoupEvent.Invoke(this);
            
            player.milOps += modifier + card.ops; 

            if (enemyInfluenceToRemove > 0)
                country.AdjustInfluence(player.enemyPlayer.faction, -enemyInfluenceToRemove);
            if (influenceToAdd > 0)
                country.AdjustInfluence(player.faction, influenceToAdd);

            coupEvent.Invoke(this);

            twilightStruggle.UI.UI_Message.SetMessage($"{modifier + card.ops}-Op +{roll.Value} Coup ({(modifier > 0 ? "+" + modifier : modifier)} {country.name} [{country.Stability * 2}]");

            if (country.Battleground)
                Game.AdjustDEFCON(-1); // Todo: Move this to a "Game Rule" that just listens for things like Nuke Subs. 
        }
    }

    public class CoupAttempt
    {
        public int ops { get; private set; }
        public Card card { get; private set; }
        public Roll roll { get; private set; }
        public Player player { get; private set; }
        public Country country { get; private set; }

        public bool successful => coupValue > 0;
        public int coupValue => roll.Value + card.ops - coupDefense;
        public int coupDefense => country.Stability * 2;

        public CoupAttempt(Country country, Player player, Card card, Roll roll)
        {
            this.country = country;
            this.player = player;
            this.card = card;
            this.roll = roll; 
        }
    }
}