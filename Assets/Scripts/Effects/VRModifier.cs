using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class VRModifier : Modifier
{
    [SerializeField] Continent SE_Asia;
    [SerializeField] Player USSR; 

    public override bool Applies(PlayerAction cardAction)
    {
        if (cardAction.Player != USSR)
            return false;
        else if (cardAction is Coup coup)
            return coup.attempts.All(attempt => attempt.country.Continents.Contains(SE_Asia)); 
        else if (cardAction is Place place)
            return place.PlacedCountries.All(c => c.Continents.Contains(SE_Asia));
        else if (cardAction is Realign realign)
            return realign.RealignedCountries.All(c => c.Continents.Contains(SE_Asia)); 
        return false; 
    }
}
