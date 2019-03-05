using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using System;

namespace Venture.Data
{
    public class GameData
    {
        public const string DATE_TIME_FORMAT = "yyyymmddhhmm";

        public DatabaseReference Database;
        public User User;
        public Character Character;
        public City City;
        public string UserId;

        public GameData()
        {
            // Configure database for unity editor
#if UNITY_EDITOR
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
            FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
            FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
            FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
#endif

            // Init Google sign-in 
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
                RequestIdToken = true,
                UseGameSignIn = false
            };

            Database = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task Register(string userId, Character.CharacterMeta characterMeta)
        {
            Dictionary<string, object> updates = new Dictionary<string, object>();

            // Create a new character document
            string characterId = Database.Child("meta/character").Push().Key;
            updates["/meta/character/" + characterId] = characterMeta.ToDictionary();

            // Create a new user document
            User.UserMeta userMeta = new User.UserMeta
            {
                characterId = characterId
            };
            updates["/meta/user/" + userId] = userMeta.ToDictionary();

            await Database.UpdateChildrenAsync(updates);

            // Init user and character meta
            User = new User(userId, userMeta);
            Character = new Character(characterId, characterMeta);
        }

        public async Task Login()
        {
            Game.Instance.Console.Print("Entered Data.Login()");

#if UNITY_EDITOR
            UserId = "UNITY_EDITOR";
#else
            // Exchange Google id token for firebase user
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            
            // Get Google id token with Google sign-in
            string googleIdToken = (await GoogleSignIn.DefaultInstance.SignIn()).IdToken;
            Game.Instance.Console.Print("ID Token: " + googleIdToken);


            Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
            Game.Instance.Console.Print("Credential: " + credential);

            FirebaseUser firebaseUser = await auth.SignInWithCredentialAsync(credential);
            Game.Instance.Console.Print("Firebase User Id: " + firebaseUser.UserId);

            UserId = firebaseUser.UserId;
#endif

            // Login failed
            if (UserId == null)
                return;

            // User is not registered if they don't exists in database
            DataSnapshot userSnapshot = await Database.Child("meta/user/" + UserId).GetValueAsync();
            if (!userSnapshot.Exists)
                return;

            // Init user meta
            Dictionary<string, object> userMeta = userSnapshot.Value as Dictionary<string, object>;
            User = new User(UserId, new User.UserMeta { characterId = userMeta["characterId"] as string });

            // Init character meta
            DataSnapshot characterSnapshot = await Database.Child("meta/character/" + User.Meta.characterId).GetValueAsync();
            Dictionary<string, object> characterMeta = characterSnapshot.Value as Dictionary<string, object>;
            Character = new Character(characterSnapshot.Key, new Character.CharacterMeta
            {
                firstName = (string)characterMeta["firstName"],
                lastName = (string)characterMeta["lastName"],
                prefix = (Character.CharacterPrefix)(long)characterMeta["prefix"]
            });
        }
    }
}