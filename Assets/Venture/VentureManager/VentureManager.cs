using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class VentureManager : MonoBehaviour
{
    public static VentureManager Instance = null;
    public static GoogleSignInUser GoogleUser = null;
    public static FirebaseUser FirebaseUser = null;
    public static DatabaseReference Database;
    public Transform Canvas;
    public GameObject[] Forms;

    void Awake()
    {
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
            Instantiate(Forms[0], Canvas);

        //Unity editor temporary user 
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
    
        Database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        Player player = new Player("Querulous Yogi", "Iptos");
        player.CreateNewPlayerData("12345");
    }

    // void instantiateForm(int index)
    // {
    // 	GameObject form = Instantiate(forms[index]);
    //     form.transform.SetParent(canvas);
    // 	form.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    // }
}