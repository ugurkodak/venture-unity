using UnityEngine;
using UnityEngine.UI;

namespace Venture
{
    // Prints console messages on a letter
    public class ViewportLetterConsole : MonoBehaviour
    {
        ScrollRect scrollRect;
        RectTransform content;
        Text outputText;

        void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.content;
            outputText = content.GetComponent<Text>();

            Game.Instance.Console.MessageRecieved += OnConsoleMessageRecieved;
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