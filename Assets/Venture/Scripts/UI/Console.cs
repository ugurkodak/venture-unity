using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
	ScrollRect scrollRect;
	RectTransform content;
	Text outputText;
	List<string> messages;

	public string initialMessage;

	void Awake()
	{
		scrollRect = GetComponent<ScrollRect>();
		content = scrollRect.content;
		outputText = content.GetComponent<Text>();
		messages = new List<string>();
		Print(initialMessage);
	}

	public void Print(string message)
	{
		if (messages.Count > 50) //Output buffer 50
			messages.RemoveAt(0);
		messages.Add(message);
		string txt = "";
		foreach (string s in messages)
            txt += "*" + s + "\n";			
        outputText.text = txt;
		scrollRect.verticalNormalizedPosition = .0f; //This isn't set accurately set to zero 
	}
}