using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    [Serializable]
    public abstract class PlayerAction : ISelectable
    {
        public string name;
        public Player Player => player;
        public List<Effect> requiredEffects = new();
        public List<Effect> prohibitedEffects = new();

        protected PlayerAction previousAction;
        private Player player;

        public void SetPlayer(Player player) => this.player = player; 

        public virtual bool Can(Player player, Card card) => Can();
        public virtual bool Can() =>
            UI.PlayerBoard.currentPlayer == player &&
            requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect)) &&
            !prohibitedEffects.Any(prohibitedEffects => Game.activeEffects.Contains(prohibitedEffects));

        public virtual Task Event(PlayerAction previousAction)
        {
            this.previousAction = previousAction;
            return Event(previousAction.Player);
        }
        public abstract Task Action();
        public async virtual Task Event(Player player)
        {
            this.player = player;
            /*
             * Cards that Impact Ops Values dynamically: 

            // Subjective Cumulative Turn Modifier
            * Containment [+1 all ops max: 4]
            * Red Scare/Purge [-1 All Ops min: 1]
            * Brezhnev [+1 all ops max: 4]

            // Conditional Card Modifier
            * The China Card [+1 all ops whill all asia]

            // Conditional TUrn Modifier
            * Vietnam Revolts [+1 all ops while all se asia]
            */

            // Right here we check for ALL OPS Modifiers (Brezhnev, Containment, RedScare Purge)
            // We do this once and forget it. 

            // Coup Roll Adjustments
            //*Latin American Death Squads[+1 Coup Roll friendly, -1 Coup Roll Unfriendly rest of Turn in CA / SA]
            //* SALT[-1 all Coup Rolls rest of Turn]

            // Realignment Roll Adjustments:
            //* Iran - Contra Scandal[US - 1 Realignment Rolls]

            //>> DONT FORGET: Shuttle Diplomacy and Formossan Resolution

            await Action();
        }
        
        public virtual PlayerAction Clone() => (PlayerAction)this.MemberwiseClone();
        
        public event Action<ISelectable> selectionEvent
        {
            add { }
            remove { }
        }
        public void Select()
        {
            throw new NotImplementedException();
        }
    }
}