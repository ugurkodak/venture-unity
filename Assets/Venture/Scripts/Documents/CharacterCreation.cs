using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
	Button buttonSubmit;
	InputField fieldFirstName;
	InputField fieldLastName;
	Dropdown dropdownWorld;

	void Awake()
	{
		buttonSubmit = transform.Find("Content").Find("ButtonSubmit").GetComponent<Button>();
		fieldFirstName = transform.Find("Content").Find("Form").Find("FieldFirstName").GetComponent<InputField>();
		fieldLastName = transform.Find("Content").Find("Form").Find("FieldLastName").GetComponent<InputField>();
		dropdownWorld = transform.Find("Content").Find("Form").Find("DropdownWorld").GetComponent<Dropdown>();

		buttonSubmit.onClick.AddListener(onSubmit);
		//Debug.Log(buttonSubmit.transform.name);
	}

	void onSubmit()
	{
		//Validate
		Venture.Console.Print("Hello world.");

	}
}
