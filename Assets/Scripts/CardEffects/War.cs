using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class War : CardEffect
{
    [SerializeField] List<Country> targetCountries;
    [SerializeField] int rollRequired, vpAward, milOpsAward; 

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event for {card.name} received");
        if (targetCountries.Count == 1)
            War(targetCountries.First());
        else
        {
            Debug.Log("Select Target Country");
            CountrySelectionManager selection = new CountrySelectionManager(targetCountries, War);

            while (selection.selectedCountries.Count < 1)
                await Task.Yield();

            selection.CloseSelectionManager(); 
        }

        void War(Country country)
        {
            Faction playingFaction = card.Faction ?? player.faction;
            Player povPlayer = player.faction == playingFaction ? player : player.enemyPlayer;

            int roll = Random.Range(0, 6) + 1;
            int adjustment = Game.countries.Count(c => country.Data.neighbors.Contains(c.Data) && c.control == playingFaction.enemyFaction);

            Debug.Log($"Roll of {roll} - {adjustment}: {roll - adjustment}/{rollRequired} {(roll - adjustment) >= rollRequired}");

            povPlayer.AdjustMilOps(milOpsAward); 

            if (roll - adjustment >= rollRequired)
            {
                country.AdjustInfluence(playingFaction, country.Influence(playingFaction.enemyFaction));
                country.SetInfluence(playingFaction.enemyFaction, 0);

                if (vpAward > 0)
                    (player.faction == playingFaction ? player : player.enemyPlayer).AdjustVP(vpAward);
            }
        }
    }
}
