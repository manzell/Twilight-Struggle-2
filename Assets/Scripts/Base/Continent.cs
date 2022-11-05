using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class Continent : ScriptableObject
    {
        public enum ControlStatus { None, Presence, Domination, Control }

        public int defconRestriction;
        public Color color;
        public List<Country> countries => Game.Countries.Where(country => country.Continents.Contains(this)).ToList();

        public ControlStatus GetControlStatus(Player player, ScoreCard.ScoringAttempt attempt)
        {
            bool playerMoreBGs = attempt.Battlegrounds[player] > attempt.Battlegrounds[player.Enemy];
            bool playerMoreCountries = attempt.CountryCount[player] > attempt.CountryCount[player.Enemy];
            bool playerAnyNonBGs = attempt.countryCount[player].Any(country => country.IsBattleground == false);
            bool playerALLBgs = countries.Where(c => c.IsBattleground == true).All(c => c.Control == player.Faction);

            if (playerALLBgs && playerMoreCountries) return ControlStatus.Control;
            else if (playerMoreBGs && playerMoreCountries && playerAnyNonBGs) return ControlStatus.Domination;
            else if (attempt.CountryCount[player] > 0) return ControlStatus.Presence;
            else return ControlStatus.None;
        }
    }
}