using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class VRModifier : Modifier
    {
        [SerializeField] Continent SE_Asia;
        [SerializeField] Player USSR;

        public override bool Applies(PlayerAction cardAction)
        {
            Debug.Log("Checking if Vietnam Revolts applies!");
            if (cardAction.Player != USSR)
                return false;
            else if (cardAction is Coup coup)
                return coup.Attempts.All(attempt => attempt.country.Continents.Contains(SE_Asia));
            else if (cardAction is Place place)
                return place.Placements.All(country => country.Continents.Contains(SE_Asia));
            else if (cardAction is Realign realign)
                return realign.Attempts.All(attempt => attempt.country.Continents.Contains(SE_Asia));
            return false;
        }
    }
}