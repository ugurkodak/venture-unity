using UnityEngine;

namespace Venture.Managers
{
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

		public void SetupSession()
		{
			Data.Access.Root.Child("players").Child(Game.Instance.UserId).GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					if (task.Result.Exists)
					{
						FirstName = task.Result.Child("FirstName").Value as string;
						LastName = task.Result.Child("LastName").Value as string;
						WorldId = task.Result.Child("WorldId").Value as string;
						Game.Instance.SwitchScene((int)Game.Scenes.World);
					}
					else
						Document.Instance.Open(Document.Instance.CharacterCreation);
				}
				else
					Debug.Log("Error at SetupPlayerSession()");
			});
		}

		public void CreateNewData()
		{
			Data.Access.Root.Child("players").Child(Game.Instance.UserId)
			.SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					Game.Instance.Console.Print("Registration successful.");
					Game.Instance.SwitchScene((int)Game.Scenes.World);
				}
			});
		}
	}
}