using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Venture.Prefabs.Documents
{
	public class CharacterCreation : MonoBehaviour
	{
		Button button_Submit;
		InputField field_FirstName;
		InputField field_LastName;
		Dropdown dropdown_World;

		List<string> worldIds = new List<string>();

		void Awake()
		{
			button_Submit = transform.Find("Content").Find("ButtonSubmit").GetComponent<Button>();
			field_FirstName = transform.Find("Content").Find("Form").Find("FieldFirstName").GetComponent<InputField>();
			field_LastName = transform.Find("Content").Find("Form").Find("FieldLastName").GetComponent<InputField>();
			dropdown_World = transform.Find("Content").Find("Form").Find("DropdownWorld").GetComponent<Dropdown>();
			dropdown_World.ClearOptions();
			PopulateWorlds();
			button_Submit.onClick.AddListener(async () =>
			{
				Document.Instance.Submit();
				await Character.Instance.Data.Create(
				field_FirstName.text,
				field_LastName.text,
				worldIds[dropdown_World.value],
				User.Instance.Data.Document.Key);
				await User.Instance.UpdateActiveCharacter(Character.Instance.Data.Document.Key);
				SceneManager.LoadScene("World");
			});
		}

		private async void PopulateWorlds()
		{
			var worlds = await Data.Access.Root.Child("WorldInfo").GetValueAsync();
			if (worlds.Exists)
			{
				List<string> names = new List<string>();
				foreach (var world in worlds.Children)
				{
					names.Add(world.Child("Name").Value.ToString());
					worldIds.Add(world.Key);
				}
				dropdown_World.AddOptions(names);
			}
			else
				Debug.LogError("Fail: Could't read world names");
		}
	}
}