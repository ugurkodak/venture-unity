using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Venture.Managers
{
	public class Game : MonoBehaviour
	{
		public static Game Instance = null;
		public Console Console = null;
		public enum Scenes : int { Login, World, City };

		//public GoogleSignInUser GoogleUser = null;
		//public FirebaseUser FirebaseUser = null; //This is used for character setup(authorization)
		//public string DEBUG_USER_ID = "123456789";

		void Awake()
		{
			//Persistent singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);

			//GoogleSignIn.Configuration = new GoogleSignInConfiguration
			//{
			//	WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
			//	RequestIdToken = true,
			//	UseGameSignIn = false
			//};
		}

		void Start()
		{
			//if (User.Instance.Data == null)
				//Document.Instance.Open(Document.Instance.SignIn);
		}

		public void LoadScene(int scene)
		{
			switch (scene)
			{
				case (int)Scenes.Login:
					SceneManager.LoadScene("Login");
					break;
				case (int)Scenes.World:
					SceneManager.LoadScene("World");
					break;
				case (int)Scenes.City:
					SceneManager.LoadScene("City");
					break;
				default:
					Debug.LogError("Undefined scene.");
					break;
			}
		}
	}
}
