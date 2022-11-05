using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public struct GameState
    {
        Dictionary<Country, Dictionary<Faction, int>> countryInfluence;
        List<Card> discardedCards, removedCards, drawDeck;
        Dictionary<Faction, List<Card>> cardsInHand;
        List<Effect> activeEffects;
    }
}