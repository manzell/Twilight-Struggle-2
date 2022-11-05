using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

namespace TwilightStruggle
{
    public class UI_Realign : MonoBehaviour
    {
        [SerializeField] GameObject realignPanel;
        [SerializeField] TextMeshProUGUI header, friendlyRoll, friendlyMod, enemyRoll, enemyModifier, finalInfluenceChange;
        [SerializeField] TextMeshProUGUI friendlyHeader, enemyHeader;
        [SerializeField] Button confirmButton;

        Realign.RealignAttempt attempt;

        TaskCompletionSource<Realign.RealignAttempt> task;

        private void Awake()
        {
            Realign.realignEvent += Setup;
        }

        public void Setup(Realign.RealignAttempt attempt)
        {
            this.attempt = attempt;
            header.text = $"{attempt.player.name} Realign in {attempt.country.name}";

            friendlyHeader.text = $"{attempt.player.name} Roll";
            friendlyRoll.text = attempt.friendlyRoll.Value.ToString();
            friendlyRoll.color = attempt.player.Faction.controlColor;
            friendlyMod.text = attempt.friendlyMod.ToString();

            enemyHeader.text = $"{attempt.player.Enemy.name} Roll";
            enemyRoll.text = attempt.enemyRoll.Value.ToString();
            enemyRoll.color = attempt.player.Enemy.Faction.controlColor;
            enemyModifier.text = attempt.enemyMod.ToString();

            finalInfluenceChange.text = "-" + attempt.influenceToRemove.ToString();
            if (attempt.influenceToRemove > 0)
                finalInfluenceChange.color = attempt.player.Enemy.Faction.controlColor;
            if (attempt.influenceToRemove < 0)
                finalInfluenceChange.color = attempt.player.Faction.controlColor;

            task = attempt.realignCompletion;
            realignPanel.SetActive(true);
        }

        public void Close()
        {
            task.SetResult(attempt);
            realignPanel.SetActive(false);
        }
    }
}