using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks; 
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UI_Coup : MonoBehaviour
{
    [SerializeField] GameObject coupPanel; 
    [SerializeField] TextMeshProUGUI header, opsValue, roll, coupDefense, enemyInfluenceRemoved, friendlyInfluenceAdded;
    [SerializeField] Button confirmButton;

    TaskCompletionSource<Coup.CoupAttempt> task; 

    private void Awake()
    {
        Coup.prepareCoupEvent += Setup;
    }

    Coup.CoupAttempt attempt; 

    public void Setup(Coup.CoupAttempt attempt)
    {
        this.attempt = attempt; 
        header.text = $"{attempt.player.name} Coup in {attempt.country}";
        opsValue.text = $"{attempt.ops}";
        roll.text = $"{attempt.roll.Value}";
        coupDefense.text = $"{attempt.coupDefense}";
        enemyInfluenceRemoved.text = "-" + (attempt.enemyInfluenceRemoved > 0 ? attempt.enemyInfluenceRemoved.ToString() : string.Empty);
        friendlyInfluenceAdded.text = attempt.influenceToAdd > 0 ? attempt.influenceToAdd.ToString() : "-";

        friendlyInfluenceAdded.color = attempt.player.faction.controlColor;
        enemyInfluenceRemoved.color = attempt.player.Enemy.faction.controlColor; 

        task = attempt.coupCompletion; 
        coupPanel.SetActive(true);
    }

    public void Close()
    {
        task?.SetResult(attempt); 
        coupPanel.SetActive(false); 
    }
}