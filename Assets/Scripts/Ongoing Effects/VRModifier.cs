using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class VRModifier : Modifier
{
    [SerializeField] Continent SE_Asia;
    [SerializeField] Player USSR; 

    public override bool Applies(GameAction gameAction)
    {
        if (gameAction.player != USSR) 
            return false; 
        if (gameAction is ITargetCountry t1)
            return t1.TargetCountry.Continents.Contains(SE_Asia);
        if (gameAction is ITargetCountries t2)
            return t2.TargetCountries.All(c => c.Continents.Contains(SE_Asia));
        return false; 
    }
}
