using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro; 

public class UI_Realign : MonoBehaviour
{
    [SerializeField] GameObject realignPanel;
    [SerializeField] TextMeshProUGUI header, friendlyRoll, friendlyMod, enemyRoll, enemyModifier, finalInfluenceChange; 
    [SerializeField] Button confirmButton;

    TaskCompletionSource<Realign.RealignAttempt> task;

    private void Awake()
    {
        Realign.realignEvent += Setup;
    }

    public void Setup(Realign.RealignAttempt attempt)
    {
        header.text = $"{attempt.player.name} Realign in {attempt.country}";

        friendlyRoll.text = attempt.friendlyRoll.Value.ToString();
        friendlyMod.text = attempt.friendlyMod.ToString(); 
        enemyRoll.text = attempt.enemyRoll.Value.ToString();
        enemyModifier.text = attempt.enemyMod.ToString();

        finalInfluenceChange.text = attempt.TotalRoll.ToString();

        if (attempt.TotalRoll > 0)
            finalInfluenceChange.color = attempt.player.faction.controlColor;
        if (attempt.TotalRoll < 0)
            finalInfluenceChange.color = attempt.player.enemyPlayer.faction.controlColor;

        task = attempt.realignCompletion;
        realignPanel.SetActive(true);
    }

    public void Close()
    {
        task.SetResult(null);
        realignPanel.SetActive(false);
    }
}
