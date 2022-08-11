using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Linq;


public class UI_Hand : MonoBehaviour
{
    public Player currentPlayer;

    [SerializeField] RawImage playerBoard;
    [SerializeField] Image factionIcon;
    [SerializeField] TextMeshProUGUI factionName, VP, MilOps;

    [SerializeField] GameObject cardPrefab, cardArea;

    private void Start()
    {
        Game.cardDrawEvent += OnCardDraw;
        Game.adjustMilOpsEvent += SetPlayerMilOps;
        Game.adjustVPevent += SetPlayerVP;
        Game.cardPlayEvent += (p, c) => RemoveCardFromHand(c);

        if (currentPlayer != null)
            SetPlayer(currentPlayer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SetPlayer(Game.Players.First(faction => faction != currentPlayer));
    }

    public void OnCardDraw(Player player, Card card)
    {
        if (player == currentPlayer)
            Instantiate(cardPrefab, cardArea.transform).GetComponent<UI_Card>().Setup(card);
    }

    public void RemoveCardFromHand(Card card) => Destroy(card.ui.gameObject); 

    public void ClearHand()
    {
        foreach (UI_Card card in GetComponentsInChildren<UI_Card>(true).Where(c => c.gameObject.activeSelf == false))
            Destroy(card.gameObject); 

        foreach (UI_Card card in GetComponentsInChildren<UI_Card>())
            card.transform.SetParent(currentPlayer.transform);
    }

    public void DrawPlayerHand(Player player)
    {
        foreach (Card card in player.hand)
            OnCardDraw(player, card);
    }

    public void SetPlayer(Player player)
    {
        currentPlayer = player;
        factionName.text = player.Faction.name;

        factionIcon.sprite = player.Faction.factionIcon; 
        playerBoard.color = player.Faction.controlColor; 

        SetPlayerVP(player, 0);
        SetPlayerMilOps(player, 0); 

        ClearHand();
        DrawPlayerHand(player);
    }

    void SetPlayerVP(Player player, int x) { if (player == currentPlayer) VP.text = (player.victoryPoints - player.Enemy.victoryPoints).ToString(); }
    void SetPlayerMilOps(Player player, int x) { if (player == currentPlayer) MilOps.text = player.milOps.ToString(); }
}
