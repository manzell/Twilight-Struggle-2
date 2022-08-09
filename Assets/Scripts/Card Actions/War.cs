using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class War : PlayerAction
{
    [SerializeField] List<CountryData> targetCountries = new();
    [SerializeField] int rollRequired, vpAward, milOpsAward;

    protected override async Task Action()
    {
        if (targetCountries.Count == 1)
            War(targetCountries.First().country);
        else
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select Target for {Card.name}");
            SelectionManager<Country> selectionManager = new(targetCountries.Select(data => data.country), War);
            await selectionManager.Selection;
            selectionManager.Close();
        }

        void War(Country country)
        {
            Faction playingFaction = Card.Faction ?? Player.faction;
            Player povPlayer = Player.faction == playingFaction ? Player : Player.enemyPlayer;

            int roll = Random.Range(0, 6) + 1;
            int adjustment = Game.Countries.Count(c => country.Data.neighbors.Contains(c.Data) && c.Control == playingFaction.enemyFaction);

            Debug.Log($"Roll of {roll} - {adjustment}: {roll - adjustment}/{rollRequired} {(roll - adjustment) >= rollRequired}");

            povPlayer.AdjustMilOps(milOpsAward); 

            if (roll - adjustment >= rollRequired)
            {
                twilightStruggle.UI.UI_Message.SetMessage($"Success: Roll {roll}-{adjustment} vs Target of {rollRequired}");
                country.AdjustInfluence(playingFaction, country.Influence(playingFaction.enemyFaction));
                country.SetInfluence(playingFaction.enemyFaction, 0);

                if (vpAward > 0)
                    (Player.faction == playingFaction ? Player : Player.enemyPlayer).AdjustVP(vpAward);
            }
        }
    }
}
