using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    [CreateAssetMenu]
    public class SpaceStage : ScriptableObject
    {
        [SerializeField] int requiredRoll, requiredOps;
        [SerializeField] int[] vpAwards;
        public IEnumerable<int> VPAwards => vpAwards;
        public int RequiredOps => requiredOps;
        public int RequiredRoll => requiredRoll;

        public List<Player> accomplished { get; private set; }

        private void OnEnable()
        {
            accomplished = new();
        }

        public void Accomplish(Player player)
        {
            if (vpAwards.Length > accomplished.Count)
                player.AdjustVP(vpAwards[accomplished.Count]);

            if (accomplished.Count == 0)
            {
                // Give the player the Power
            }
            else if (accomplished.Count == 1)
            {
                // Remove the power from player 1
            }

            accomplished.Add(player);
        }
    }
}