using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;
using Sirenix.OdinInspector; 

public class UI_Country : SerializedMonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI countryName, countryStability;
    [SerializeField] TextMeshProUGUI usInfluence, ussrInfluence;
    [SerializeField] Image battlegroundIndicator, usInfluenceBG, ussrInfluenceBG;
    [SerializeField] Image background, border, highlight;
    [SerializeField] Color influenceBGcolor, textColor; 

    public event Action<Country> onClickHandler;
    Player USA, USSR;

    public void OnPointerClick(PointerEventData eventData) => onClickHandler.Invoke(GetComponent<Country>());

    void Start()
    {
        USA = FindObjectsOfType<Player>().First(player => player.name == "USA");
        USSR = FindObjectsOfType<Player>().First(player => player.name == "USSR");

        if (GetComponent<Country>() != null)
            Setup(GetComponent<Country>());
    }

    public void Setup(Country country)
    {
        country.ui = this; 
        gameObject.name = country.name; 
        countryName.text = country.name;
        countryStability.text = country.Stability.ToString();
        battlegroundIndicator.color = country.Battleground ? Color.red : Color.black;
        background.color = country.Continents.First().color;

        country.onInfluencePlacementEvent += () => UpdateUI(country);
        UpdateUI(country); 
    }

    public void UpdateUI(Country country)
    {
        usInfluence.text = country.Influence(USA) == 0 ? string.Empty : country.Influence(USA).ToString(); 
        ussrInfluence.text = country.Influence(USSR) == 0 ? string.Empty : country.Influence(USSR).ToString();

        usInfluence.color = country.control == USA.faction ? Color.white : USA.faction.controlColor;
        usInfluenceBG.color = country.control == USA.faction ? country.control.controlColor : influenceBGcolor;
        ussrInfluence.color = country.control == USSR.faction ? Color.white : USSR.faction.controlColor;
        ussrInfluenceBG.color = country.control == USSR.faction ? country.control.controlColor : influenceBGcolor;
    }

    Color lastColor; 
    public void SetHighlight(Color color) 
    {
        lastColor = highlight.color; 
        highlight.color = color; 
        highlight.gameObject.SetActive(true); 
    }

    public void ResetHighlight() 
    {
        Color c = highlight.color; 
        highlight.color = lastColor;
        lastColor = c;
        highlight.gameObject.SetActive(false);
    }

    public void ClearHighlight() 
    { 
        highlight.color = Color.white; 
        highlight.gameObject.SetActive(false); 
    }
}