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

		//void Start()
		//{
		//	TestCRUD();
		//}

		public async Task SignIn()
		{
#if UNITY_EDITOR
			string id = UNITY_EDITOR_USER_ID;
#else
			var FirebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
				GoogleAuthProvider.GetCredential((await GoogleSignIn.DefaultInstance.SignIn()).IdToken, null));
			string id = FirebaseUser.UserId;
#endif
			if (await Data.Read(id))
			{
				if (Data.ActiveCharacterId == null)
					Document.Instance.Open(Document.Instance.CharacterCreation);
				else
					SceneManager.LoadScene("World");
			}
			else
			{
				await Data.Create(id);
				Document.Instance.Open(Document.Instance.CharacterCreation);
			}
			await UpdateLastSignIn();
		}

		public void SignOut()
		{
			FirebaseAuth.DefaultInstance.SignOut();
			GoogleSignIn.DefaultInstance.SignOut();
		}

		public async Task UpdateActiveCharacter(string characterId)
		{
			Data.ActiveCharacterId = characterId;
			await Data.Update();
		}

		public async Task UpdateLastSignIn()
		{
			Data.LastSignIn = DateTime.Now.ToString(Venture.Data.Access.DATE_TIME_FORMAT);
			await Data.Update();
		}

		public async void TestCRUD()
		{
			Debug.Log("--- USER CRUD TEST ---");
			Debug.Log("Creating new user in database with debug id");
			await Data.Create(UNITY_EDITOR_USER_ID);
			Debug.Log("Reading user from database");
			await Data.Read(UNITY_EDITOR_USER_ID);
			Debug.Log("Deleting user");
			await Data.Delete();
			Debug.Log("Done");
		}
	}
}