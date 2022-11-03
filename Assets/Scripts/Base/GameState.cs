using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameState
{
    Dictionary<Country, Dictionary<Faction, int>> countryInfluence;
    List<Card> discardedCards, removedCards, drawDeck;
    Dictionary<Faction, List<Card>> cardsInHand;
    List<Effect> activeEffects;
}
