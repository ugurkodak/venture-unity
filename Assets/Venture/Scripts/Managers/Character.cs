using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
	public static Character Instance;
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

	public void SetupCharacterSession()
	{
		Venture.Database.Child("players").Child(Venture.UserId).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					FirstName = task.Result.Child("FirstName").Value as string;
					LastName = task.Result.Child("LastName").Value as string;
					WorldId = task.Result.Child("WorldId").Value as string;
					Venture.Instance.CreateWorldTiles();
				}
				else
					Document.Instance.Open(Document.Instance.CharacterCreation);
			}
			else
				Debug.Log("Error at SetupPlayerSession()");
		});
	}

	public void CreateNewCharacterData()
	{
		Venture.Database.Child("players").Child(Venture.UserId)
		.SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				Venture.Console.Print("Registration successful.");
				Venture.Instance.CreateWorldTiles();
			}
		});
	}
}
