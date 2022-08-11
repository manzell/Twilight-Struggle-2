using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using System.Threading.Tasks;

public class War : PlayerAction
{
    public static event Action<WarAttempt> prepareWar, warAttempt;
    [SerializeField] List<CountryData> targetCountries = new();
    [SerializeField] int rollRequired, vpAward, milOpsAward;

    public int RollRequired => rollRequired;
    public int VPAward => vpAward;
    public int MilOps => milOpsAward; 

    protected override async Task Action()
    {
        if (targetCountries.Count == 1)
            War(targetCountries.First().country);
        else
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select Target for {Card.name}");
            SelectionManager<Country> selectionManager = new(targetCountries.Select(data => data.country), War);
            War(await selectionManager.Selection); 
            selectionManager.Close();
        }

        async void War(Country country)
        {
            WarAttempt attempt = new WarAttempt(Player, this, country, rollRequired);
            await attempt.warCompletion.Task; 
            attempt.player.AdjustMilOps(milOpsAward); 

            if (attempt.success)
            {
                Faction faction = Card.Faction ?? Player.faction;
                country.AdjustInfluence(faction, country.Influence(faction.enemyFaction));
                country.SetInfluence(faction.enemyFaction, 0);

                if (vpAward > 0)
                    faction.player.AdjustVP(vpAward); 
            }
        }
    }

    public class WarAttempt
    {
        public TaskCompletionSource<WarAttempt> warCompletion;
        public Player player; 
        public Country country;
        public Roll roll;
        public War war; 
        public int modifier, rollRequired;
        public bool success => roll.Value - modifier > rollRequired; 

        public WarAttempt(Player player, War war, Country country, int rollRequired)
        {
            prepareWar?.Invoke(this);
            warCompletion = new();
            this.player = war.Player ?? player; 
            this.country = country;
            this.war = war;
            this.roll = new(0); 
            this.rollRequired = rollRequired;
            modifier = country.Neighbors.Count(cd => cd.country.Control == player.Enemy);

            Debug.Log($"Roll of {roll.Value} - {modifier}: {roll.Value - modifier}/{rollRequired} {(roll.Value - modifier) >= rollRequired}");
            warAttempt?.Invoke(this);
        }
    }
}
