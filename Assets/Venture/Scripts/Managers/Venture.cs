using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Google;
using UnityEngine;
using System.Collections.Generic;

public class Venture : MonoBehaviour
{
    public static Venture Instance = null;
	public static Console Console = null;
    public static GoogleSignInUser GoogleUser = null;
    public static FirebaseUser FirebaseUser = null;
    public static DatabaseReference Database;
	public static string UserId = "123456789"; //TODO: There is a better way probably

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

        //UNITY_EDITOR User 
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
    
        Database = FirebaseDatabase.DefaultInstance.RootReference;
    }

	void Start()
	{
		if (GoogleUser == null)
			Document.Instance.Open(Document.Instance.SignIn);
	}

	//Size = floor(n^2/4)
	public void CreateWorldTiles()
	{
		Database.Child("tiles").Child(Character.Instance.WorldId).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					foreach (var position in task.Result.Children)
					{
						GameObject tile = new GameObject(position.Key);
						tile.transform.position = new Vector3(
						float.Parse((position.Value as IDictionary<string, object>)["x"].ToString()),
						float.Parse((position.Value as IDictionary<string, object>)["y"].ToString()),
						float.Parse((position.Value as IDictionary<string, object>)["z"].ToString()));
					}
				}
			}
		});
	}
}