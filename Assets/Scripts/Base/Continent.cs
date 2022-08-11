using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Continent : ScriptableObject
{
    public enum ControlStatus { None, Presence, Domination, Control }

    public int defconRestriction;
    public Color color;
    public List<Country> countries => Game.Countries.Where(country => country.Continents.Contains(this)).ToList();

    [SerializeField]
    public ControlStatus GetControlStatus(Player player)
    {
        int factionControlledCountries = countries.Count(country => country.Control == player.Faction);
        int enemyControlledCountries = countries.Count(country => country.Control != null && country.Control == player.Faction); 
        int factionBattlegrounds = countries.Count(country => country.Battleground && country.Control == player.Faction);
        int enemyBattlegrounds = countries.Count(country => country.Battleground && country.Control != null && country.Control != player.Faction);

        Debug.Log($"{name} Control Status: {player.name}; Friendly {factionBattlegrounds}/{factionControlledCountries} Enemy {enemyBattlegrounds}/{enemyControlledCountries}");

        if (factionBattlegrounds == countries.Count(country => country.Battleground) && factionControlledCountries > enemyControlledCountries)
            return ControlStatus.Control;

        if (factionControlledCountries > enemyControlledCountries && factionBattlegrounds > enemyBattlegrounds)
            return ControlStatus.Domination;

        if (factionControlledCountries > 0)
            return ControlStatus.Presence;

        return ControlStatus.None;
    }
}
