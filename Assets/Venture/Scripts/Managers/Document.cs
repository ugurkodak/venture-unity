using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Document : MonoBehaviour
{
	public static Document Instance;
	public GameObject Canvas;
	public GameObject SignIn;
	public GameObject CharacterCreation;
	public GameObject Settings;
	public GameObject Character;

	GameObject current;

	void Awake()
	{
		//Singleton
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		current.GetComponent<Animator>().SetTrigger("open");
	}

	public void Open(GameObject document)
	{
		current = Instantiate(document, Canvas.transform);
		Venture.Console = current.GetComponentInChildren<Console>();
		current.GetComponent<Animator>().SetTrigger("open");
	}

	public void Submit()
	{
		current.GetComponent<Animator>().SetTrigger("submit");
	}

	//public void Discard()
	//{
	//	current.GetComponent<Animator>().SetTrigger("discard");
	//}

	//public void OpenSignIn()
	//{
	//	current = Instantiate(SignIn, Canvas.transform);
	//	Venture.Console = current.GetComponentInChildren<Console>();
	//	current.GetComponent<Animator>().SetTrigger("open");
	//}

	//public void OpenSettings()
	//{
	//	current = Instantiate(Settings);
	//}

	//public void OpenCharacterCreation(string id)
	//{
	//	current = Instantiate(CharacterCreation);
	//}
}
