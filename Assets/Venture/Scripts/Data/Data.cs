using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Google;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Venture.Data
{
    public class GameData
    {
        public const string DATE_TIME_FORMAT = "yyyymmddhhmm";

        private FirebaseApp firebaseApp;
        private FirebaseAuth firebaseAuth;
        private FirebaseDatabase firebaseDatabase;
        private GoogleSignIn googleSignIn;

        public User User;
        public Character Character;
        public City City;
        public string UserId;

        public GameData()
        {
            // Init Firebase
            firebaseApp = FirebaseApp.DefaultInstance;
            firebaseAuth = FirebaseAuth.DefaultInstance;
            firebaseDatabase = FirebaseDatabase.DefaultInstance;
#if UNITY_EDITOR
            // Unity Editor setup for testing with restricted database access
            firebaseApp.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
            firebaseApp.SetEditorP12FileName("Venture-9af379c14c56.p12");
            firebaseApp.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
            firebaseApp.SetEditorP12Password("notasecret");
#else
            // Init Google Sign-In
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
                RequestIdToken = true,
                UseGameSignIn = false
            };

            googleSignIn = GoogleSignIn.DefaultInstance; // Instance needs to init after configuration
#endif
        }

        // Force firstname lastname combination to be unique
        public async Task Register(string userId, CharacterMeta characterMeta)
        {
            Dictionary<string, object> updates = new Dictionary<string, object>();

            // Create a new character document
            string characterId = firebaseDatabase.RootReference.Child("meta/character").Push().Key;
            updates["/meta/character/" + characterId] = characterMeta.ToDictionary();

            // Create a new user document
            UserMeta userMeta = new UserMeta
            {
                characterId = characterId
            };
            updates["/meta/user/" + userId] = userMeta.ToDictionary();

            await firebaseDatabase.RootReference.UpdateChildrenAsync(updates);

            // Init user and character meta
            User = new User(userId, userMeta);
            Character = new Character(characterId, characterMeta);
        }

        public async Task Login()
        {
#if UNITY_EDITOR
            UserId = "UNITY_EDITOR";
#else
            // Get Google id token with Google sign-in, exchange it for firebase credential,
            // sing in with firebase, save UserId
            UserId = (await firebaseAuth.SignInWithCredentialAsync(
                GoogleAuthProvider.GetCredential(
                    (await GoogleSignIn.DefaultInstance.SignIn()).IdToken, null))).UserId;
#endif

            Game.Instance.Console.Print("User Id: " + UserId);

            // Login failed
            if (UserId == null)
                return;

            // User is not registered if they don't exists in database

            DataSnapshot userSnapshot = await firebaseDatabase.RootReference.Child("meta/user/" + UserId).GetValueAsync();
            if (userSnapshot == null) // Sometimes(or always?) GetValueAsync returns null
                return;
            if (!userSnapshot.Exists)
                return;

            // Init user meta
            Dictionary<string, object> userMeta = userSnapshot.Value as Dictionary<string, object>;
            User = new User(UserId, new UserMeta { characterId = userMeta["characterId"] as string });

            // Init character meta
            DataSnapshot characterSnapshot = await firebaseDatabase.RootReference.Child("meta/character/" + User.Meta.characterId).GetValueAsync();
            Dictionary<string, object> characterMeta = characterSnapshot.Value as Dictionary<string, object>;
            Character = new Character(characterSnapshot.Key, new CharacterMeta
            {
                firstName = (string)characterMeta["firstName"],
                lastName = (string)characterMeta["lastName"],
                prefix = (CharacterPrefix)(long)characterMeta["prefix"]
            });
        }

        public async Task<List<string>> GetCharacterFirstNames()
        {
            //TODO: Get names from database
            List<string> names = new List<string>()
            {
                "Anne", "Wolfgang", "Ludwig", "Marie", "John", "George", "Andrew", "Catherine", "Louis"
            };

            return names;
        }

        public async Task<List<string>> GetCharacterLastNames()
        {
            //TODO: Get names from database
            List<string> names = new List<string>()
            {
                "Franklin", "Jackson", "Kant", "Burns", "Rousseau", "Handel", "Goya", "Haydn", "Bentham"
            };

            return names;
        }
    }
}