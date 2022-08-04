using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class NORADEffect : Effect
{
    [SerializeField] Country canada;
    [SerializeField] Player USA;
    CountrySelectionManager selection;
    bool trip; 

    public override bool Test(GameAction t) => canada.control == USA && Game.currentPhase is ActionRound;

    public override void Apply()
    {
        Game.DEFCONAdjust += OnDEFCONAdjust;
    }

    void OnDEFCONAdjust(int adjust)
    {
        if(trip == false && adjust < 0 && Game.DEFCON == 2 && Game.currentPhase is ActionRound actionRound)
        {
            actionRound.phaseEndEvent += StartPlacement;
            trip = true; 
        }
    }

    void StartPlacement()
    {
        selection = new(Game.countries.Where(country => country.Influence(USA) > 0), EndPlacement); 
    }

    void EndPlacement(Country country)
    {
        country.AdjustInfluence(USA.faction, 1);
        selection.CloseSelectionManager();
    }
}
