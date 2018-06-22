using UnityEngine;
using Firebase.Auth;
using Google;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;

namespace Venture
{
	public class User : MonoBehaviour
	{
		public const string UNITY_EDITOR_USER_ID = "123456789";
		public static User Instance;
		public Data.User Data;

		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			Data = new Data.User();

			GoogleSignIn.Configuration = new GoogleSignInConfiguration
			{
				WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
				RequestIdToken = true,
				UseGameSignIn = false
			};
		}

		void Start()
		{
			Document.Instance.Open(Document.Instance.SignIn);
		}

		public async Task SetupInstance()
		{
#if UNITY_EDITOR
			string id = UNITY_EDITOR_USER_ID;
#else
			var FirebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
				GoogleAuthProvider.GetCredential((await GoogleSignIn.DefaultInstance.SignIn()).IdToken, null));
			string id = FirebaseUser.UserId;
#endif
			await Data.Load(id);
			if (Data.DateCreated != null)
			{
				await Data.UpdateLastSignIn();
				if (Data.ActiveCharacterKey == null)
					Document.Instance.Open(Document.Instance.CharacterCreation);
				else
				{
					await Character.Instance.SetupInstance(Data.ActiveCharacterKey);
					SceneManager.LoadScene("World");
				}
			}
			else
			{
				Data.Create(id);
				await Data.Update();
				Document.Instance.Open(Document.Instance.CharacterCreation);
			}
		}

		//TODO: Do we need sign out?
		//public void SignOut()
		//{
		//	FirebaseAuth.DefaultInstance.SignOut();
		//	GoogleSignIn.DefaultInstance.SignOut();
		//}
	}
}