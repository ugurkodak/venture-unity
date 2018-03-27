using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Google;
using Firebase.Auth;

public class SignInButton : MonoBehaviour
{
    public VentureConsole console;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToggleAuthorization);
        //button.onClick.Invoke();
    }

    void ToggleAuthorization()
    {
        if (VentureManager.googleUser == null)
        {
            try
            {
                gameObject.GetComponentInChildren<Text>().text = "Working...";
                //Google authentication
                GoogleSignIn.DefaultInstance.SignIn().ContinueWith((Task<GoogleSignInUser> googleAuthTask) =>
                {
                    if (googleAuthTask.IsCompleted)
                    {
                        VentureManager.googleUser = googleAuthTask.Result;
                        console.Print("Google authentication successfull.");
                        //Firebase authentication
                        FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
                            GoogleAuthProvider.GetCredential(VentureManager.googleUser.IdToken, null))
                        .ContinueWith(firebaseAuthTask =>
                        {
                            if (firebaseAuthTask.IsCompleted)
                            {
                                VentureManager.firebaseUser = firebaseAuthTask.Result;
                                console.Print("Firebase authentication successfull.");
                                gameObject.GetComponentInChildren<Text>().text = VentureManager.firebaseUser.DisplayName;
                            }
                            else
                            {
                                VentureManager.firebaseUser = null;
                                console.Print("Firebase authentication failed.");
                            }
                        });
                    }
                    else
                    {
                        VentureManager.googleUser = null;
                        gameObject.GetComponentInChildren<Text>().text = "Sign in";
                        console.Print("Unexpected authentication error.");
                    }
                });
            }
            catch
            {
                console.Print("Crashed while signing in.");
            }
        }
        else
        {
            GoogleSignIn.DefaultInstance.SignOut();
            FirebaseAuth.DefaultInstance.SignOut();
            VentureManager.googleUser = null;
            gameObject.GetComponentInChildren<Text>().text = "Sign in";
            console.Print("Signed out. Please sign in again.");
        }
    }
}
