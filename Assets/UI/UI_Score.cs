using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace TwilightStruggle
{
    public class UI_Score : MonoBehaviour
    {
        ScoreCard.ScoringAttempt attempt;

        [SerializeField] GameObject scorePanel;
        [SerializeField] TextMeshProUGUI header;
        [SerializeField] TextMeshProUGUI friendlyFaction, friendlyBGs, friendlyCountries, friendlyAdjacent;
        [SerializeField] TextMeshProUGUI enemyFaction, enemyBGs, enemyCountries, enemyAdjacent;
        [SerializeField] TextMeshProUGUI outcome;

        private void Awake()
        {
            ScoreCard.scoringEvent += Setup;
        }

        public void Setup(ScoreCard.ScoringAttempt attempt)
        {
            Player enemyPlayer = attempt.Player.Enemy;
            int VP = attempt.VP(attempt.Player) - attempt.VP(enemyPlayer);

            this.attempt = attempt;

            header.text = $"{attempt.Player.name} scores {attempt.Continent.name}";
            friendlyFaction.text = attempt.Player.name;
            friendlyFaction.color = attempt.Player.Faction.controlColor;
            friendlyBGs.text = attempt.Battlegrounds[attempt.Player].ToString();
            friendlyCountries.text = attempt.CountryCount[attempt.Player].ToString();
            friendlyAdjacent.text = attempt.AdjacentSuperpowers[attempt.Player].ToString();
            enemyFaction.text = enemyPlayer.name;
            enemyFaction.color = enemyPlayer.Faction.controlColor;
            enemyBGs.text = attempt.Battlegrounds[enemyPlayer].ToString();
            enemyCountries.text = attempt.CountryCount[enemyPlayer].ToString();
            enemyAdjacent.text = attempt.AdjacentSuperpowers[enemyPlayer].ToString();

            if (VP > 0)
            {
                outcome.text = $"{attempt.Player.name} {attempt.ControlStatus(attempt.Player)} (+{VP})";
                outcome.color = attempt.Player.Faction.controlColor;
            }
            else if (VP < 0)
            {
                outcome.text = $"{enemyPlayer.name} {attempt.ControlStatus(enemyPlayer)} (+{Mathf.Abs(VP)})";
                outcome.color = enemyPlayer.Faction.controlColor;
            }

            scorePanel.SetActive(true);
        }

        public void Close()
        {
            attempt.Close();
            scorePanel.SetActive(false);
        }
    }
}