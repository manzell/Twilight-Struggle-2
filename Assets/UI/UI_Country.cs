using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;
using Sirenix.OdinInspector;

public class UI_Country : SerializedMonoBehaviour, IPointerClickHandler, IHighlightable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI countryName, countryStability;
    [SerializeField] TextMeshProUGUI usInfluence, ussrInfluence;
    [SerializeField] Image battlegroundIndicator, usInfluenceBG, ussrInfluenceBG;
    [SerializeField] Image background, border, highlight;
    [SerializeField] Color influenceBGcolor, textColor;

    public Country country { get; private set; }
    public event Action<Country> onClickHandler;

    Player USA, USSR;
    Color lastColor;

    void Start()
    {
        USA = FindObjectsOfType<Player>().First(player => player.name == "USA");
        USSR = FindObjectsOfType<Player>().First(player => player.name == "USSR");

        if (TryGetComponent(out Country country))
            Setup(country);
    }

    public void Setup(Country country)
    {
        this.country = country;
        country.SetUI(this);
        gameObject.name = country.name;
        countryName.text = country.name;
        countryStability.text = country.Stability.ToString();
        battlegroundIndicator.color = country.Battleground ? Color.red : Color.black;
        background.color = country.Continents.First().color;

        country.onInfluencePlacementEvent += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        usInfluence.text = country.Influence(USA) == 0 ? string.Empty : country.Influence(USA).ToString();
        ussrInfluence.text = country.Influence(USSR) == 0 ? string.Empty : country.Influence(USSR).ToString();

        usInfluence.color = country.Control == USA.Faction ? Color.white : USA.Faction.controlColor;
        usInfluenceBG.color = country.Control == USA.Faction ? country.Control.controlColor : influenceBGcolor;
        ussrInfluence.color = country.Control == USSR.Faction ? Color.white : USSR.Faction.controlColor;
        ussrInfluenceBG.color = country.Control == USSR.Faction ? country.Control.controlColor : influenceBGcolor;
    }

    public void Show() => highlight.gameObject.SetActive(true);
    public void Hide() => highlight.gameObject.SetActive(false);

    public void SetHighlight(Color color)
    {
        lastColor = highlight.color;
        highlight.color = color;
    }

    public void ResetHighlight()
    {
        Color c = highlight.color;
        highlight.color = lastColor;
        lastColor = c;
    }

    public void ClearHighlight()
    {
        lastColor = Color.clear;
        highlight.color = Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)  => onClickHandler?.Invoke(country);
    public void OnPointerEnter(PointerEventData eventData) { if (highlight.color.a > 0) SetHighlight(Color.yellow); }
    public void OnPointerExit(PointerEventData eventData) => ResetHighlight();
}

public interface ISelectable
{
    public event Action<ISelectable> selectionEvent;
    public abstract void Select(); 
}

public interface IHighlightable
{
    public void SetHighlight(Color color);
    public void ClearHighlight();
}