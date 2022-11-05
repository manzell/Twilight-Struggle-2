using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class AddDeck : PhaseAction
    {
        public Deck deck;

        public override Task Do(Phase phase)
        {
            foreach (CardData card in deck.cards)
                Game.drawDeck.Add(new Card(card));

            Game.drawDeck = Game.drawDeck.OrderBy(card => Random.value).ToList();

            return Task.CompletedTask;
        }
    }
}