using UnityEngine;
using Firebase.Auth;
using Google;
using System;
using Newtonsoft.Json;

namespace Venture.Data
{
	public class User
	{
		public const string UNITY_EDITOR_USER_ID = "123456789";
		private readonly string id;
		public string JoinDate { get; private set; }
		//User can only have one active character but abandoned 
		//characters persist in simulation until the world end date
		public string ActiveCharacterId { get; private set; }
		public string LastLogin { get; private set; }
		//TODO: User stats

		public User(string id)
		{
			this.id = id;
		}

		public async void Create()
		{
			var user = await Access.Root.Child("users/" + id).GetValueAsync();
			if (!user.Exists)
			{
				JoinDate = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
				Update();
				Debug.Log("New user.");
			}
			else
				Debug.LogError("User already exists.");
		}

		public async void Update()
		{
			await Access.Root.Child("users/" + id)
			.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public async void Read()
		{
			var user = await Access.Root.Child("users/" + id).GetValueAsync();
			if (user.Exists)
			{
				ActiveCharacterId = user.Child("ActiveCharacterId").GetRawJsonValue();
				JoinDate = user.Child("JoinDate").GetRawJsonValue();
				LastLogin = user.Child("LastLogin").GetRawJsonValue();
			}
			else
				Debug.LogError("User doesn't exist.");
		}

		public void Delete()
		{

		}

		public void UpdateActiveCharacter(string characterId)
		{
			ActiveCharacterId = characterId;
			Update();
		}

		public void UpdateLastLogin()
		{
			LastLogin = DateTime.Now.ToString(Data.Access.DATE_TIME_FORMAT);
			Update();
		}
	}
}

namespace Venture.Managers //TODO: Move file into managers and create a manager prefab
{
	public class User : MonoBehaviour
	{
		public static User Instance;
		public GoogleSignInUser GoogleUser;
		public FirebaseUser FirebaseUser; //This is used for character setup(authorization) TODO:Probably remove these
		public Data.User Data;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);

			GoogleUser = null;
			FirebaseUser = null;
			Data = new Data.User(Venture.Data.User.UNITY_EDITOR_USER_ID); //TODO: Remove

			GoogleSignIn.Configuration = new GoogleSignInConfiguration
			{
				WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
				RequestIdToken = true,
				UseGameSignIn = false
			};
		}

		private void Start()
		{
			Data.Create();
			//Data.Pull(Venture.Data.User.UNITY_EDITOR_USER_ID);
		}

		public async void SignIn(Action continuation)
		{
			Instance.GoogleUser = await GoogleSignIn.DefaultInstance.SignIn();
			Instance.FirebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
				GoogleAuthProvider.GetCredential(Instance.GoogleUser.IdToken, null));
			Data.UpdateLastLogin();
			continuation();
		}

		public void SignOut()
		{
			FirebaseAuth.DefaultInstance.SignOut();
			GoogleSignIn.DefaultInstance.SignOut();
			Instance.GoogleUser = null;
			Instance.FirebaseUser = null;
		}
	}
}
