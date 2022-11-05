using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwilightStruggle.UI
{
    public class UI_TurnCounter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI turnNumber, arMarker;

        private void Start() => Phase.PhaseStartEvent += Setup;

        public void Setup(Phase phase)
        {
            if (phase.Get<Turn>() != null)
                turnNumber.text = phase.Get<Turn>().turnNumber.ToString();
            if (phase.Get<ActionRound>() != null)
            {
                arMarker.text = $"AR{phase.Get<ActionRound>().actionRoundNumber}";
                arMarker.color = phase.Get<ActionRound>().phasingPlayer.Faction.controlColor;
            }
        }
    }
}