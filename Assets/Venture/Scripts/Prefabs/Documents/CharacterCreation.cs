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

		List<string> worldIds = new List<string>();

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
			//Read all worlds meta and fill dropdown with names
			Data.Access.Root.Child("worlds/meta").GetValueAsync().ContinueWith(worlds =>
			{
				if (worlds.IsCompleted && worlds.Exception == null && worlds.Result.Exists)
				{
					List<string> names = new List<string>();
					
					foreach (var world in worlds.Result.Children)
					{
						names.Add(world.Child("Name").Value.ToString());
						worldIds.Add(world.Key);
					}
					dropdownWorld.AddOptions(names);
				}
				else
					Debug.Log("Fail: Could't read world names");
			});
		}

		void OnSubmit()
		{
			//TODO: Validate
			//TODO: Check how many players are registered in the world and either lock it down or
			//create new city.
			Character.Instance.data.Create(
				fieldFirstName.text,
				fieldLastName.text,
				worldIds[dropdownWorld.value],
#if UNITY_EDITOR 
				Game.Instance.DEBUG_USER_ID)
#else
				Game.Instance.FirebaseUser.UserId)
#endif
				.Write();
			Character.Instance.SetupSession();

		}
	}
}
