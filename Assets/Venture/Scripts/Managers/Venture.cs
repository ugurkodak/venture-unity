using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Venture : MonoBehaviour
{
    public static Venture Instance = null;
	public static Console Console = null;
    public static GoogleSignInUser GoogleUser = null;
    public static FirebaseUser FirebaseUser = null;
    public static DatabaseReference Database;

	void Awake()
    {
		//Singleton
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
		if (GoogleUser == null)
			Document.Instance.Open(Document.Instance.SignIn);

        //Unity editor temporary user 
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
    
        Database = FirebaseDatabase.DefaultInstance.RootReference;
    }
}