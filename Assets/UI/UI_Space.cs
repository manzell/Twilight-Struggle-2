using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class UI_Space : MonoBehaviour
{
    [SerializeField] GameObject spacePanel;
    [SerializeField] TextMeshProUGUI header, required, roll, outcome; 
    [SerializeField] Button confirmButton;

    TaskCompletionSource<Space.SpaceAttempt> task;

    private void Awake()
    {
        Space.spaceAttempt += Setup; 
    }

    public void Setup(Space.SpaceAttempt attempt)
    {
        header.text = $"{attempt.player.name} attempts {attempt.stage.name}";
        required.text = attempt.stage.RequiredOps.ToString();
        roll.text = attempt.roll.Value.ToString();
        outcome.text = attempt.successful ? "SUCCESS" : "FAILURE";
        if (attempt.successful)
            outcome.color = attempt.player.faction.controlColor;

        task = attempt.spaceCompletion;
        spacePanel.SetActive(true);
    }

    public void Close()
    {
        task.SetResult(null);
        spacePanel.SetActive(false);
    }
}
