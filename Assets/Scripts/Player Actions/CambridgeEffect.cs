using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class CambridgeEffect : PlayerAction
    {
        [SerializeField] Faction USA, USSR;
        [SerializeField] WarPhase prohibitedPhase;

        public override bool Can(Player player, Card card) => Phase.GetCurrent<Turn>().warPhase != prohibitedPhase && base.Can(player, card);

        public override async Task Action()
        {
            List<Continent> eligibleContinents = new();

            foreach (Card c in USA.player.hand)
                if (c.Data is ScoringCard scoringCard)
                    eligibleContinents.Add(scoringCard.continent);

            if (eligibleContinents.Count > 0)
            {
                TwilightStruggle.UI.Message.SetMessage($"Cambridge Five. Add 1 USSR Influence in {eligibleContinents.Implode("or")}");

                SelectionManager<Country> selectionManager = new(Game.Countries.Where(country => country.Continents.Any(continent => eligibleContinents.Contains(continent))));
                Country country = await selectionManager.Selection as Country;
                selectionManager.Close();

                country.AdjustInfluence(USSR, 1);
            }
        }
    }
}