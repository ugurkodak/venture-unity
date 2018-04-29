using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Venture.Managers;

namespace Venture.Prefabs
{
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
			dropdownWorld.ClearOptions();
			buttonSubmit.onClick.AddListener(OnSubmit);
		}

		void Start()
		{
			//Fill dropdown with available worlds
			Data.Access.Root.Child("worlds").GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					if (task.Result.Exists)
					{
						List<string> worlds = new List<string>();
						foreach (var world in task.Result.Children)
							worlds.Add((world.Value as IDictionary<string, object>)["name"] as string);
						dropdownWorld.AddOptions(worlds);
					}
				}
			});
		}

		void OnSubmit()
		{
			//TODO: Validate
			Data.Access.Root.Child("worlds").OrderByChild("name")
			.EqualTo(dropdownWorld.options[dropdownWorld.value].text)
			.GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					if (task.Result.Exists)
						foreach (var world in task.Result.Children)
							Character.Instance.WorldId = world.Key;
					Character.Instance.FirstName = fieldFirstName.text;
					Character.Instance.LastName = fieldLastName.text;
					Character.Instance.CreateNewData();
					Document.Instance.Submit();
				}
			});
		}
	}
}
