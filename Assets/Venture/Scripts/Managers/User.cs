using UnityEngine;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

namespace Venture.Managers //TODO: Move file into managers and create a manager prefab
{
	public class User : MonoBehaviour
	{
		public const string UNITY_EDITOR_USER_ID = "123456789";
		public static User Instance;
		//public GoogleSignInUser GoogleUser;
		//public FirebaseUser FirebaseUser; //This is used for character setup(authorization) TODO:Probably remove
		public Data.User Data;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);

			//GoogleUser = null;
			//FirebaseUser = null;
			//Data = new Data.User(Venture.Data.User.UNITY_EDITOR_USER_ID); //TODO: Remove

			GoogleSignIn.Configuration = new GoogleSignInConfiguration
			{
				WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
				RequestIdToken = true,
				UseGameSignIn = false
			};
		}

		private void Start()
		{
			TestCRUD();
		}

		public async void TestCRUD()
		{
			Debug.Log("--- USER CRUD TEST ---");
			Debug.Log("Allocating new user data in memory with debug id.");
			Data = new Data.User(UNITY_EDITOR_USER_ID);
			Debug.Log("Creating new user in database");
			await Data.Create();
			Debug.Log("Reading user from database");
			await Data.Read();
			Debug.Log("Deleting user");
			await Data.Delete();
			Debug.Log("Done");
		}

		public async Task SignIn()
		{
#if UNITY_EDITOR
			Data = new Data.User(UNITY_EDITOR_USER_ID);
#else
			var FirebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
				GoogleAuthProvider.GetCredential((await GoogleSignIn.DefaultInstance.SignIn()).IdToken, null));
			Data = new Data.User(FirebaseUser.UserId);
#endif
			//if (await Data.Read())
			//	await Data.UpdateLastLogin();
			//else
			//	Document.Instance.Open(Document.Instance.CharacterCreation);
		}

		public void SignOut()
		{
			FirebaseAuth.DefaultInstance.SignOut();
			GoogleSignIn.DefaultInstance.SignOut();
			//Instance.GoogleUser = null;
			//Instance.FirebaseUser = null;
		}
	}
}
