using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks; 

public class NORADEffect : Effect
{
    [SerializeField] CountryData canada;
    [SerializeField] Faction USA;
    bool trip; 

    public override bool Test(PlayerAction t) => canada.country.Control == USA && Game.currentPhase is ActionRound;

    public override void Apply()
    {
        Game.DEFCONAdjust += OnDEFCONAdjust;
        ActionRound.PhaseStartEvent += phase => trip = false; 
    }

    void OnDEFCONAdjust(int adjust)
    {
        if(trip == false && adjust < 0 && Game.DEFCON == 2 && Game.currentPhase is ActionRound actionRound)
        {
            actionRound.phaseEndEvent += StartPlacement;
            trip = true; 
        }
    }

    async void StartPlacement()
    {
        SelectionManager<Country> selectionManager = new(Game.Countries.Where(country => country.Influence(USA) > 0), 
            country => country.AdjustInfluence(USA, 1));

        while (selectionManager.Selected.Count() == 0)
            await selectionManager.Selection;

        selectionManager.Close();
    }
}
