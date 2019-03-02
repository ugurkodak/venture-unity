using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Venture
{
    public class LetterConsole : MonoBehaviour
    {
        ScrollRect scrollRect;
        RectTransform content;
        Text outputText;
        // List<string> messages;

        public string initialMessage;

        void Awake()
        {
            // Game.Instance.Console = this; // TODO: temp
            // messages = new List<string>();
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.content;
            outputText = content.GetComponent<Text>();

            Game.Instance.Console.MessageRecieved += OnConsoleMessageRecieved;
            // Print(initialMessage);
        }

        public void OnConsoleMessageRecieved(object source, ConsoleEventArgs e)
        {
            string txt = "";
            foreach (string s in e.messages)
                txt += "*" + s + "\n";
            outputText.text = txt;
            scrollRect.verticalNormalizedPosition = 1;
        }
    }
}