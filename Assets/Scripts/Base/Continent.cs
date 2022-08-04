using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Continent : ScriptableObject
{
    public enum ControlStatus { None, Presence, Domination, Control }

    public int defconRestriction;
    public Color color;
    public List<Country> countries => Game.countries.Where(country => country.Continents.Contains(this)).ToList();

    [SerializeField]
    public ControlStatus GetControlStatus(Player player)
    {
        int factionControlledCountries = countries.Count(country => country.control == player.faction);
        int enemyControlledCountries = countries.Count(country => country.control != null && country.control == player.faction); 
        int factionBattlegrounds = countries.Count(country => country.Battleground && country.control == player.faction);
        int enemyBattlegrounds = countries.Count(country => country.Battleground && country.control != null && country.control != player.faction);

        if (factionBattlegrounds == countries.Count(country => country.Battleground) && factionControlledCountries > enemyControlledCountries)
            return ControlStatus.Control;

        if (factionControlledCountries > enemyControlledCountries && factionBattlegrounds > enemyBattlegrounds)
            return ControlStatus.Domination;

        if (factionControlledCountries > 0)
            return ControlStatus.Presence;

        return ControlStatus.None;
    }
}
