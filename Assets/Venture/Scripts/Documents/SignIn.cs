using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour {

	Button signInButton;

	void Awake()
	{
		signInButton = transform.Find("Content").Find("SignInButton").GetComponent<Button>();
		signInButton.onClick.AddListener(onSignIn);
	}

	void Start()
	{
		signInButton.onClick.Invoke();
	}

	void onSignIn()
	{
		gameObject.GetComponentInChildren<Text>().text = "Signing In...";
		gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
		if (Venture.GoogleUser == null)
		{
			try
			{
				//Google authentication
				GoogleSignIn.DefaultInstance.SignIn().ContinueWith((Task<GoogleSignInUser> googleAuthTask) =>
				{
					if (googleAuthTask.IsCompleted)
					{
						Venture.GoogleUser = googleAuthTask.Result;
						Venture.Console.Print("Google authentication successfull.");

						//Firebase authentication
						FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
							GoogleAuthProvider.GetCredential(Venture.GoogleUser.IdToken, null))
						.ContinueWith(firebaseAuthTask =>
						{
							if (firebaseAuthTask.IsCompleted)
							{
								Venture.FirebaseUser = firebaseAuthTask.Result;
								Venture.Console.Print("Firebase authentication successfull.");

								gameObject.GetComponentInChildren<Text>().text = "Success";
								Document.Instance.Submit();
								Character.Instance.SetupCharacterSession(Venture.FirebaseUser.UserId);
							}
							else
							{
								Venture.FirebaseUser = null;
								gameObject.GetComponentInChildren<Text>().text = "Retry";
								gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
								Venture.Console.Print("Firebase authentication failed.");
							}
						});
					}
					else
					{
						Venture.GoogleUser = null;
						gameObject.GetComponentInChildren<Text>().text = "Retry";
						gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
						Venture.Console.Print("Google authentication failed.");
					}
				});
			}
			catch
			{
				if (Venture.EDITOR)
				{
					gameObject.GetComponentInChildren<Text>().text = "DEBUG";
					Document.Instance.Submit();
					Character.Instance.SetupCharacterSession("12345");
				}
				else
					Venture.Console.Print("Crashed while signing in.");
			}
		}
		else
		{
			FirebaseAuth.DefaultInstance.SignOut();
			GoogleSignIn.DefaultInstance.SignOut();
			Venture.GoogleUser = null;
			Venture.FirebaseUser = null;
			gameObject.GetComponentInChildren<Text>().text = "Sign in";
			Venture.Console.Print("Signed out. Please sign in again.");
		}
	}
}
