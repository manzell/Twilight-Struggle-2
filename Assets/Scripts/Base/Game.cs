using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{
    [SerializeField] Phase rootPhase;

    public static int DEFCON = 5;
    public static Phase currentPhase;
    public static List<Effect> activeEffects = new();
    public static List<Modifier> modifiers = new();
    public static List<Player> Players { get; private set; }
    public static List<Country> Countries { get; private set; }
    public static event Action<int> OnDefconAdjust;
    public static event Action<Player, int> adjustMilOpsEvent, adjustVPevent;
    public static event Action<Player, Card> CardDrawEvent;
    public static event Action<Player, Card> CardPlayEvent;

    public static void AdjustVPEvent(Player player, int i) => adjustVPevent(player, i);
    public static void AdjustMilOpEvent(Player player, int i) => adjustMilOpsEvent(player, i);

    private void Awake()
    {
        Players = GetComponentsInChildren<Player>().ToList();
        Countries = FindObjectsOfType<Country>().ToList();
    }

    private void Start()
    {
        StartGame();
    }

    public async void StartGame()
    {
        if(currentPhase == null)
            currentPhase = rootPhase;

        await Task.Delay(1000); // Just wait 1 second for whatever reason
        
        currentPhase.StartPhase(null);
    }

    public static void EndGame()
    {
        Debug.Log("Game Over");
        Application.Quit(); 
    }

    #region DEFCON
    public static void AdjustDEFCON(int d)
    {
        int oldDefcon = DEFCON;
        int newDefcon = Mathf.Clamp(DEFCON + d, 1, 5);

        if(oldDefcon != newDefcon)
        {
            Debug.Log($"{(newDefcon > oldDefcon ? "Upgrading" : "Degrading")} DEFCON by {Mathf.Abs(newDefcon - oldDefcon)}");
            DEFCON = newDefcon;
            OnDefconAdjust?.Invoke(newDefcon - oldDefcon); 
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
            CardDrawEvent?.Invoke(player, card);
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

    public static void InvokePlayEvent(Player player, Card card) => CardPlayEvent?.Invoke(player, card); 

    #endregion
}