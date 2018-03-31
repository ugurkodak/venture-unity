using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignInButton : MonoBehaviour
{
	Button button;

	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(SignIn);
	}

	void Start()
	{
		button.onClick.Invoke();
	}

	public void SignIn()
	{
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

								Character.Instance.SetupCharacterSession(Venture.FirebaseUser.UserId);
								gameObject.GetComponentInChildren<Text>().text = "Success.";
								Document.Instance.Submit();
							}
							else
							{
								Venture.FirebaseUser = null;
								Venture.Console.Print("Firebase authentication failed.");
							}
						});
					}
					else
					{
						Venture.GoogleUser = null;
						gameObject.GetComponentInChildren<Text>().text = "Sign in";
						Venture.Console.Print("Unexpected authentication error.");
					}
				});
			}
			catch
			{
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
