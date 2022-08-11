using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class UI_War : MonoBehaviour
{

    [SerializeField] GameObject warPanel;
    [SerializeField] TextMeshProUGUI header, roll, modifier, required, outcome, vpAward; 
    [SerializeField] Button confirmButton;

    TaskCompletionSource<War.WarAttempt> task;

    private void Awake()
    {
        War.warAttempt += Setup; 
    }

    public void Setup(War.WarAttempt attempt)
    {
        header.text = attempt.war.Card.name; 
        roll.text = attempt.roll.Value.ToString();
        required.text = attempt.rollRequired.ToString(); 
        modifier.text = attempt.modifier.ToString();
        outcome.text = attempt.success ? attempt.player.name : attempt.player.Enemy.name + " Wins!";

        List<string> awards = new(); 
        if (attempt.war.VPAward > 0 && attempt.success)
            awards.Add($"+{attempt.war.VPAward} {(attempt.war.VPAward == 1 ? "VP" : "VPs")}");
        if (attempt.war.MilOps > 0)
            awards.Add($"+{attempt.war.MilOps} {(attempt.war.MilOps == 1 ? "MilOp" : "MilOps")}");

        vpAward.text = $"{attempt.player.name} {awards.Implode(",")}";
        vpAward.color = attempt.player.faction.controlColor;

        task = attempt.warCompletion;
        warPanel.SetActive(true);
    }
    
    public void Close()
    {
        task.SetResult(null);
        warPanel.SetActive(false);
    }
}
