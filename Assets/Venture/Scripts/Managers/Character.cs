using UnityEngine;

namespace Venture.Managers
{
	public class Character : MonoBehaviour
	{
		public static Character Instance;
		public Data.Character Data;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			Data = new Data.Character();
		}

		public void SetupSession()
		{
			Venture.Data.Access.Root.Child("characters")
			.OrderByChild("UserId")
			.EqualTo(User.Instance.Data.ActiveCharacterId)
			.GetValueAsync()
			.ContinueWith(task => 
			{
				var val = task.Result.Value as string;
				//Debug.Log(val);
				//Debug.Log(task.Result.Key);
				//Debug.Log(task.Result.Exists);
				//Debug.Log(task.Result.GetRawJsonValue());
			});


//			Venture.Data.Access.Root.Child("characters")
//			.OrderByChild("UserId")
//#if UNITY_EDITOR
//			.EqualTo(Game.Instance.DEBUG_USER_ID)
//#else
//			.EqualTo(Game.Instance.FirebaseUser.UserId)
//#endif
//			.GetValueAsync().ContinueWith(task =>
//			{
//				if (task.IsCompleted && task.Exception == null)
//				{
//					if (task.Result.Exists)
//					{
//#if UNITY_EDITOR
//						Debug.Log(task.Result);
//						//Data.Read(task.Result.ChildrenCount);
//#else
//						Data.Read(Game.Instance.FirebaseUser.UserId);
//#endif
//						Game.Instance.LoadScene((int)Game.Scenes.World);
//					}
//					else
//						Document.Instance.Open(Document.Instance.CharacterCreation);
//				}
//				else
//					Debug.Log("Fail: Could't set up character session");
//			});
		}
	}
}