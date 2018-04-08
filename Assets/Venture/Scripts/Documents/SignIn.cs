using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
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
		if (Venture.Instance.GoogleUser == null)
		{
			try
			{
				//Google authentication
				GoogleSignIn.DefaultInstance.SignIn().ContinueWith((Task<GoogleSignInUser> googleAuthTask) =>
				{
					if (googleAuthTask.IsCompleted)
					{
						Venture.Instance.GoogleUser = googleAuthTask.Result;
						Venture.Instance.Console.Print("Google authentication successfull.");

						//Firebase authentication
						FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
							GoogleAuthProvider.GetCredential(Venture.Instance.GoogleUser.IdToken, null))
						.ContinueWith(firebaseAuthTask =>
						{
							if (firebaseAuthTask.IsCompleted)
							{
								Venture.Instance.FirebaseUser = firebaseAuthTask.Result;
								Venture.Instance.UserId = Venture.Instance.FirebaseUser.UserId; //TODO: There is a better way probably
								Venture.Instance.Console.Print("Firebase authentication successfull.");
								gameObject.GetComponentInChildren<Text>().text = "Success";
								Document.Instance.Submit();
								Character.Instance.SetupCharacterSession();
							}
							else
							{
								Venture.Instance.FirebaseUser = null;
								gameObject.GetComponentInChildren<Text>().text = "Retry";
								gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
								Venture.Instance.Console.Print("Firebase authentication failed.");
							}
						});
					}
					else
					{
						Venture.Instance.GoogleUser = null;
						gameObject.GetComponentInChildren<Text>().text = "Retry";
						gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
						Venture.Instance.Console.Print("Google authentication failed.");
					}
				});
			}
			catch
			{
#if UNITY_EDITOR
				gameObject.GetComponentInChildren<Text>().text = "DEBUG";
				Document.Instance.Submit();
				Character.Instance.SetupCharacterSession();
#else
				Venture.Instance.Console.Print("Crashed while signing in.");
#endif
			}
		}
		else
		{
			FirebaseAuth.DefaultInstance.SignOut();
			GoogleSignIn.DefaultInstance.SignOut();
			Venture.Instance.GoogleUser = null;
			Venture.Instance.FirebaseUser = null;
			gameObject.GetComponentInChildren<Text>().text = "Sign in";
			Venture.Instance.Console.Print("Signed out. Please sign in again.");
		}
	}
}
