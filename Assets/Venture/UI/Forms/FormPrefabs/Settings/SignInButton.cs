using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Google;

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
        if (VentureManager.user == null)
        {
            try
            {
                gameObject.GetComponentInChildren<Text>().text = "Working...";
                GoogleSignIn.DefaultInstance.SignIn().ContinueWith((Task<GoogleSignInUser> task) =>
                {
                    if (task.IsCompleted)
                    {
                        VentureManager.user = task.Result;
                        gameObject.GetComponentInChildren<Text>().text = VentureManager.user.DisplayName;
                        console.Print("Authentication successfull.");
                    }
                    else
                    {
                        VentureManager.user = null;
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
            VentureManager.user = null;
            gameObject.GetComponentInChildren<Text>().text = "Sign in";
            console.Print("Signed out. Please sign in again.");
        }
    }
}
