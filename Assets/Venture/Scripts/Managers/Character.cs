using UnityEngine;

public class Character : MonoBehaviour
{
	public static Character Instance;

	public string Id;
	public string FirstName;
	public string LastName;
	public string WorldId;

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
					Instance.Id = id;
					Instance.FirstName = task.Result.Child("firstName").Value as string;
					Instance.LastName = task.Result.Child("lastName").Value as string;
					Instance.WorldId = task.Result.Child("worldId").Value as string;
				}
				else
				{
					Instance.Id = id;
					Document.Instance.Open(Document.Instance.CharacterCreation);
				}
			}
			else
				Debug.Log("Error at SetupPlayerSession()");
		});
	}

	public void CreateNewCharacterData(string id, string firstName, string lastName, string worldId)
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
