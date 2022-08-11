using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Linq; 

public class UI_Score : MonoBehaviour
{
    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI header; 
    [SerializeField] TextMeshProUGUI friendlyFaction, friendlyBGs, friendlyCountries, friendlyAdjacent;
    [SerializeField] TextMeshProUGUI enemyFaction, enemyBGs, enemyCountries, enemyAdjacent;
    [SerializeField] TextMeshProUGUI outcome;

    TaskCompletionSource<ScoreCard> task;
    ScoreCard scoring; 

    private void Awake()
    {
        ScoreCard.scoringEvent += Setup;
    }

    public void Setup(ScoreCard scoring)
    {
        this.scoring = scoring; 
        header.text = $"{scoring.Player.name} scores {scoring.Continent}";

        Player enemyPlayer = scoring.Player.Enemy; 

        friendlyFaction.text = scoring.Player.name;
        friendlyFaction.color = scoring.Player.Faction.controlColor;
        friendlyBGs.text = scoring.Battlegrounds[scoring.Player].ToString();
        friendlyCountries.text = scoring.Continent.countries.Count(country => country.Control == scoring.Player.Faction).ToString();

        enemyFaction.text = enemyPlayer.name;
        enemyFaction.color = enemyPlayer.Faction.controlColor;
        enemyBGs.text = scoring.Battlegrounds[enemyPlayer].ToString();
        enemyCountries.text = scoring.Continent.countries.Count(country => country.Control == enemyPlayer.Faction).ToString();

        int VP = scoring.VP(scoring.Player) - scoring.VP(enemyPlayer);

        if (scoring.VP(scoring.Player) > scoring.VP(enemyPlayer))
        {
            outcome.text = $"{scoring.Player.name} {scoring.Continent.GetControlStatus(scoring.Player)} " +
                $"(+{VP})";
            outcome.color = scoring.Player.Faction.controlColor; 
        }
        else if(scoring.VP(scoring.Player) < scoring.VP(enemyPlayer))
        {
            outcome.text = $"{enemyPlayer.name} {scoring.Continent.GetControlStatus(enemyPlayer)} " +
                $"(+{Mathf.Abs(VP)})";
            outcome.color = enemyPlayer.Faction.controlColor;
        }

        task = scoring.scoringTask; 
        scorePanel.SetActive(true);
    }

    public void Close()
    {
        task.SetResult(scoring);
        scorePanel.SetActive(false);
    }
}
