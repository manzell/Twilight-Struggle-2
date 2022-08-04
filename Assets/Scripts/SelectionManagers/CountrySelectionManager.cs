using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq; 

public class CountrySelectionManager
{
    public List<Country> SelectableCountries => selectableCountries;

    List<Country> selectableCountries;
    public List<Country> selectedCountries; 
    System.Action<Country> callback;
    public bool finishedSelectingCountries; 

    public CountrySelectionManager(IEnumerable<Country> availableCountries, System.Action<Country> callback)
    {
        selectableCountries = new(availableCountries); 
        this.callback = callback;

        AddHiglights(selectableCountries.Select(s => s.ui));
        AddClickListener(selectableCountries.Select(s => s.ui));
        finishedSelectingCountries = false; 
    }

    void AddClickListener(IEnumerable<UI_Country> countries)
    {
        foreach (UI_Country country in countries)
        {
            country.onClickHandler += callback;
            country.onClickHandler += country => { if (!selectedCountries.Contains(country)) selectedCountries.Add(country); }; 
        }
    }

    void AddHiglights(IEnumerable<UI_Country> countries)
    {
        foreach(UI_Country country in countries)
            country.SetHighlight(Color.red);
    }

    void RemoveHiglight(IEnumerable<UI_Country> countries)
    {
        foreach (UI_Country country in countries) 
            RemoveHiglight(country);
    }

    void RemoveHiglight(UI_Country country)
    {
        country.onClickHandler -= callback;
        country.SetHighlight(Color.clear);
    }

    public void AddSelectableCountries(List<Country> countries) => AddHiglights(countries.Select(country => country.ui)); 
    public void RemoveSelectableCountries(List<Country> countries) => RemoveHiglight(countries.Select(country => country.ui));
    public void RemoveSelectableCountry(Country country) => RemoveHiglight(country.ui);

    public void CloseSelectionManager()
    {
        RemoveSelectableCountries(selectableCountries); 
    }
}
