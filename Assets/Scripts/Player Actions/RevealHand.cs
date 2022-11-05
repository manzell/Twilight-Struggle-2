using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TwilightStruggle
{
    public class RevealHand : PlayerAction
    {
        public override async Task Action()
        {
            List<Card> revealedHand = Player.hand;

            SelectionManager<Card> selectionManager = new(revealedHand);
            await selectionManager.Selection;
            selectionManager.Close();
        }
    }
}