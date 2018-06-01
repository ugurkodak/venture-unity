using UnityEngine;

namespace Venture.Managers
{
	public class Character : MonoBehaviour
	{
		public static Character Instance;
		public Data.Character data;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			data = new Data.Character();
		}

		public void SetupSession()
		{
			Data.Access.Root.Child("characters")
			.OrderByChild("UserId")
#if UNITY_EDITOR
			.EqualTo(Game.Instance.DEBUG_USER_ID)
#else
			.EqualTo(Game.Instance.FirebaseUser.UserId)
#endif
			.GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted && task.Exception == null)
				{
					if (task.Result.Exists)
					{
#if UNITY_EDITOR
						data.Read(Game.Instance.DEBUG_USER_ID);
#else
						data.Read(Game.Instance.FirebaseUser.UserId);
#endif
						Game.Instance.LoadScene((int)Game.Scenes.World);
					}
					else
						Document.Instance.Open(Document.Instance.CharacterCreation);
				}
				else
					Debug.Log("Fail: Could't set up character session");
			});
		}
	}
}