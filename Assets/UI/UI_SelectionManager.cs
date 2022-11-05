using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;

namespace TwilightStruggle.UI
{
    public class UI_SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject actionSelectionPrefab, cardPrefab, actionArea;

        public void Start()
        {
            SelectionManager<Country>.SelectionStartEvent += StartCountrySelection;
            SelectionManager<Country>.SelectionEndEvent += CloseCountrySelector;
        }

        public void StartCountrySelection(SelectionManager<Country> selectionManager)
        {
            selectionManager.addSelectableEvent += SetHighlight;
            selectionManager.removeSelectableEvent += ClearHighlight;
        }

        public void CloseCountrySelector(SelectionManager<Country> selectionManager)
        {
            selectionManager.addSelectableEvent -= SetHighlight;
            selectionManager.removeSelectableEvent -= ClearHighlight;
        }

        void SetHighlight(ISelectable country) => FindObjectsOfType<UI_Country>().Where(ui => ui.country == (Country)country).First().SetHighlight(Color.red); 
        void ClearHighlight(ISelectable country) => FindObjectsOfType<UI_Country>().Where(ui => ui.country == (Country)country).First().ClearHighlight();
    }
}