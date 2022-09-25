using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using Unity.VisualScripting;

public abstract class PlayerAction : ISelectable
{
    public static UI_SelectionManager ui => GameObject.FindObjectOfType<UI_SelectionManager>();

    Player player;
    protected PlayerAction previousAction;

    public string name;
   
    public Player Player => player;
    public List<Effect> requiredEffects = new();
    public List<Effect> prohibitedEffects = new();

    public event Action<ISelectable> selectionEvent
    {
        add { }
        remove { }
    }

    public abstract Task Action();

    public PlayerAction()
    {
        name = GetType().ToString(); 
    }

    public virtual bool Can(Player player, Card card) => Can();
    public virtual bool Can() => requiredEffects.All(requiredEffect => Game.activeEffects.Contains(requiredEffect)) &&
        !prohibitedEffects.Any(prohibitedEffects => Game.activeEffects.Contains(prohibitedEffects));

    public Task Event() => Action();
    public virtual Task Event(PlayerAction previousAction)
    {
        this.previousAction = previousAction; 
        return Event(previousAction.Player);
    }

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

    public virtual void SetPlayer(Player player) => this.player = player;

    public void Select()
    {
        throw new NotImplementedException();
    }
}