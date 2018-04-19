using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Venture.Managers
{
	public class Game : MonoBehaviour
	{
		public static Game Instance = null;
		public Console Console = null;
		public GoogleSignInUser GoogleUser = null;
		public FirebaseUser FirebaseUser = null;
		public DatabaseReference DatabaseRootReference;
		public string UserId = "123456789"; //TODO: There is a better way probably

		public enum Scenes : int { Login, World, City };
		Scenes scene = Scenes.Login;

		void Awake()
		{
			//Persistent singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);

			GoogleSignIn.Configuration = new GoogleSignInConfiguration
			{
				WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
				RequestIdToken = true,
				UseGameSignIn = false
			};

			//UNITY_EDITOR User 
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
			FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
			FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
			FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

			DatabaseRootReference = FirebaseDatabase.DefaultInstance.RootReference;
		}

		void Start()
		{
			if (GoogleUser == null)
				Document.Instance.Open(Document.Instance.SignIn);
		}

		public void SwitchScene(int scene)
		{
			switch (scene)
			{
				case (int)Scenes.Login:
					SceneManager.LoadScene("Login");
					scene = (int)Scenes.Login;
					break;
				case (int)Scenes.World:
					SceneManager.LoadScene("World");
					scene = (int)Scenes.World;
					break;
				case (int)Scenes.City:
					SceneManager.LoadScene("City");
					scene = (int)Scenes.City;
					break;
				default:
					Debug.LogError("Undefined scene.");
					break;
			}
		}
	}
}
