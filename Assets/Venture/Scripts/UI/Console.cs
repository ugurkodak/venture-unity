using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		print(message);
		if (messages.Count > 50) //Output buffer 50
			messages.RemoveAt(50);
		messages.Insert(0, message);
		string txt = "";
		foreach (string s in messages)
            txt += "*" + s + "\n";			
        outputText.text = txt;
		//scrollRect.verticalNormalizedPosition = .0f; //This isn't set accurately set to zero 
	}
}