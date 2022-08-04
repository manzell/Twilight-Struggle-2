using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{
    public static List<Effect> activeEffects = new();
    public static List<Player> Players { get; private set; }
    public static List<Country> countries; 

    public static int DEFCON = 5;
    public static event Action<int> DEFCONAdjust;

    public static event Action<Player, Card> cardDrawEvent;
    public static event Action<Player, Card> cardPlayEvent;
    public static event Action<Player, int> adjustMilOpsEvent, adjustVPevent;

    public static Phase currentPhase;
    public Phase rootPhase;

    private void Awake()
    {
        Players = GetComponentsInChildren<Player>().ToList();
        countries = FindObjectsOfType<Country>().ToList(); 
    }

    private void Start()
    {
        currentPhase = rootPhase;
        currentPhase.StartPhase(null);
    }

    [ContextMenu("Advance")]
    public void Advance() => currentPhase.Continue(); 

    public static void EndGame()
    {
        Debug.Log("Game Over");
        Application.Quit(); 
    }

    public static void AdjustVP(Player player, int amt)
    {
        adjustVPevent.Invoke(player, amt); 
        // Check Victory Condition
    }

    public static void AdjustMilOps(Player player, int amount) => adjustMilOpsEvent.Invoke(player, amount); 

    #region DEFCON
    public static void AdjustDEFCON(int d)
    {
        int oldDefcon = DEFCON;
        int newDefcon = Mathf.Clamp(DEFCON + d, 1, 5);

        if(oldDefcon != newDefcon)
        {
            DEFCON = newDefcon;
            DEFCONAdjust.Invoke(newDefcon - oldDefcon); 
        }
    }

    #endregion

    #region Hand and Card Management
    public static List<Card>
        drawDeck = new(),
        discards = new(),
        removed = new();

    public static Task Draw(Player player)
    {
        if (drawDeck.Count == 0)
            Reshuffle();

        if(drawDeck.Count > 0)
        {
            Card card = drawDeck.First();

            drawDeck.Remove(card);
            player.hand.Add(card);

            Debug.Log($"{player} draws {card.name}");
            cardDrawEvent?.Invoke(player, card);
        }

        return Task.CompletedTask; 
    }

    public static void Reshuffle()
    {
        Debug.Log($"Reshuffle"); 
        drawDeck.AddRange(discards);
        discards.Clear();
        drawDeck = drawDeck.OrderBy(c => UnityEngine.Random.value).ToList(); 
    }

    public static void InvokePlayEvent(Player player, Card card) 
    {
        cardPlayEvent.Invoke(player, card); 
    }
    #endregion


}
