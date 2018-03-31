using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class Character : MonoBehaviour
{
	public static Character Instance;

	string id;
	string firstName;
	string lastName;
	string worldId;

	void Awake()
	{
		//Singleton
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	public void SetupCharacterSession(string id)
	{
		Venture.Database.Child("players").Child(id).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					Instance.id = id;
					Instance.firstName = task.Result.Child("firstName").Value as string;
					Instance.lastName = task.Result.Child("lastName").Value as string;
					Instance.worldId = task.Result.Child("worldId").Value as string;
				}
				//else
					//Document.Instance.OpenCharacterCreation(id);
			}
			else
				Debug.Log("Error at SetupPlayerSession()");
		});
	}

	public void CreateNewCharacterData(string id)
	{
		Venture.Database.Child("players").Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWith(task =>
		{
			if (task.IsCompleted)
				Debug.Log("Created new user");
		});
	}

	public void StartPlayerCreation(string id)
	{
		Debug.Log("Start player creation");
	}
}
