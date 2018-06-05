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
		//User can only have one active character but abandoned 
		//characters persist in simulation until the world end date
		public string CharacterId { get; private set; }
		public string JoinDate { get; private set; }
		//TODO: User stats

		public async void Create(string id, string characterId)
		{
			CharacterId = characterId;
			JoinDate = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			await Access.Root.Child("users/" + id)
			.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public void Create(string id)
		{
			Create(id, null);
		}

		public async void Read(string id)
		{
			Debug.Log(JoinDate);

			var user = await Access.Root.Child("users/" + id).GetValueAsync();
			JoinDate = user.Child("JoinDate").GetRawJsonValue();

			Debug.Log(JoinDate);
		}

		public void Update()
		{

		}

		public void Delete()
		{

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
			Data = new Data.User();

			GoogleSignIn.Configuration = new GoogleSignInConfiguration
			{
				WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
				RequestIdToken = true,
				UseGameSignIn = false
			};
		}

		private void Start()
		{
			//Data.Create(Venture.Data.User.UNITY_EDITOR_USER_ID);
			Data.Read(Venture.Data.User.UNITY_EDITOR_USER_ID);
			
		}

		public async void SignIn(Action continuation)
		{
			Instance.GoogleUser = await GoogleSignIn.DefaultInstance.SignIn();
			Instance.FirebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
				GoogleAuthProvider.GetCredential(Instance.GoogleUser.IdToken, null));
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
