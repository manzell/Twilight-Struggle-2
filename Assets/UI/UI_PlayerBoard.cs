using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;


public class UI_PlayerBoard : MonoBehaviour
{
    public static Player currentPlayer;
    static UI_PlayerBoard _this => FindObjectOfType<UI_PlayerBoard>();

    [SerializeField] RawImage playerBoard;
    [SerializeField] Image factionIcon;
    [SerializeField] TextMeshProUGUI factionName, VP, MilOps;
    [SerializeField] GameObject cardPrefab, cardArea;

    private void Start()
    {
        Game.CardDrawEvent += AddCard;
        Game.adjustMilOpsEvent += SetPlayerMilOps;
        Game.adjustVPevent += SetPlayerVP;
        UI_PlayerAction.cardDropEvent += (action, card) => RemoveCardFromHand(card); 

        SetPlayer(currentPlayer ?? Game.Players.First());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SetPlayer(Game.Players.First(faction => faction != currentPlayer));
    }

    public static void SetPlayer(Player player)
    {
        currentPlayer = player;

        _this.factionName.text = player.Faction.name;
        _this.factionIcon.sprite = player.Faction.factionIcon;
        _this.playerBoard.color = player.Faction.controlColor;

        if (player == currentPlayer)
            _this.VP.text = (player.victoryPoints - player.Enemy.victoryPoints).ToString();
        if (player == currentPlayer)
            _this.MilOps.text = player.milOps.ToString();
        ClearHand();
        DrawPlayerHand(player);
    }
    public static void RemoveCardFromHand(Card card)
    {
        Debug.Log($"RemoveCardFromHand(Card {card.name})");
        if(Game.Players.Any(player => player.hand.Remove(card)))
            Destroy(card.ui.gameObject);
    }
    static public void ClearHand()
    {
        foreach (Transform child in _this.cardArea.transform)
            Destroy(child.gameObject);
    }

    static void AddCard(Player player, Card card)
    {
        if (player == currentPlayer)
            Instantiate(_this.cardPrefab, _this.cardArea.transform).GetComponent<UI_Card>().Setup(card); 
    }
    static void DrawPlayerHand(Player player)
    {
        foreach (Card card in player.hand)
            AddCard(player, card);
    }
    static void SetPlayerVP(Player player, int amount) => FindObjectOfType<UI_PlayerBoard>().VP.text = (player == currentPlayer ? player.victoryPoints - player.Enemy.victoryPoints : player.Enemy.victoryPoints - player.victoryPoints).ToString();
    static void SetPlayerMilOps(Player player, int amount) => FindObjectOfType<UI_PlayerBoard>().MilOps.text = (player == currentPlayer ? player.milOps : player.Enemy.milOps).ToString(); 
}