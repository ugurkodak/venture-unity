using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Venture.Managers;

namespace Venture.Prefabs 
{
	public class SignIn : MonoBehaviour
	{
		Button signInButton;

		void Awake()
		{
			signInButton = transform.Find("Content").Find("SignInButton").GetComponent<Button>();
			signInButton.onClick.AddListener(OnSignIn);
		}

		void Start()
		{
			signInButton.onClick.Invoke();
		}

		void OnSignIn()
		{
			gameObject.GetComponentInChildren<Text>().text = "Signing In...";
			gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
			if (Game.Instance.GoogleUser == null)
			{
				try
				{
					//Google authentication
					GoogleSignIn.DefaultInstance.SignIn().ContinueWith((Task<GoogleSignInUser> googleAuthTask) =>
					{
						if (googleAuthTask.IsCompleted)
						{
							Game.Instance.GoogleUser = googleAuthTask.Result;
							Game.Instance.Console.Print("Google authentication successfull.");

							//Firebase authentication
							FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
								GoogleAuthProvider.GetCredential(Game.Instance.GoogleUser.IdToken, null))
							.ContinueWith(firebaseAuthTask =>
							{
								if (firebaseAuthTask.IsCompleted)
								{
									Game.Instance.FirebaseUser = firebaseAuthTask.Result;
									Game.Instance.Console.Print("Firebase authentication successfull.");
									gameObject.GetComponentInChildren<Text>().text = "Success";
									Document.Instance.Submit();
									Character.Instance.SetupSession();
								}
								else
								{
									Game.Instance.FirebaseUser = null;
									gameObject.GetComponentInChildren<Text>().text = "Retry";
									gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
									Game.Instance.Console.Print("Firebase authentication failed.");
								}
							});
						}
						else
						{
							Game.Instance.GoogleUser = null;
							gameObject.GetComponentInChildren<Text>().text = "Retry";
							gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
							Game.Instance.Console.Print("Google authentication failed.");
						}
					});
				}
				catch
				{
#if UNITY_EDITOR
					gameObject.GetComponentInChildren<Text>().text = "DEBUG";
					Document.Instance.Submit();
					Character.Instance.SetupSession();
#else
					Venture.Instance.Console.Print("Crashed while signing in.");
#endif
				}
			}
			else
			{
				FirebaseAuth.DefaultInstance.SignOut();
				GoogleSignIn.DefaultInstance.SignOut();
				Game.Instance.GoogleUser = null;
				Game.Instance.FirebaseUser = null;
				gameObject.GetComponentInChildren<Text>().text = "Sign in";
				Game.Instance.Console.Print("Signed out. Please sign in again.");
			}
		}
	}
}
