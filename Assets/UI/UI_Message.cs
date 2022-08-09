using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace twilightStruggle.UI
{
    public class UI_Message : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI message;
        private static UI_Message reference;

        private void Start() => reference = this;

        public static void SetMessage(string message) { reference._SetMessage(message); Debug.Log($"UI Message: {message}"); }
        public static void ClearMessage() => reference._SetMessage(string.Empty); 
        public void _SetMessage(string message) => this.message.text = message;
    }
}