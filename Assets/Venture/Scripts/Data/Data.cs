using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;

namespace Venture.Data
{
    // TODO: Error handling
    public class GameData
    {
        public enum LoadState {  NONE, USER, CHARACTER, CITY }

        public const string DATE_TIME_FORMAT = "yyyymmddhhmm";

        public DatabaseReference Database;
        public User User;
        public Character Character;
        public City City;


        public GameData()
        {
            // Init Firebase
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
            FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
            FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
            FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
            Database = FirebaseDatabase.DefaultInstance.RootReference;

            // Init Google sign-in 
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
                RequestIdToken = true,
                UseGameSignIn = false
            };
        }

        public async Task<LoadState> Register(string userId, Character.CharacterMeta characterMeta)
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

            return LoadState.CHARACTER;
        }


        public async Task<LoadState> Login()
        {
            string userId;
#if UNITY_EDITOR
            userId = "UNITY-EDITOR";
#else
            // Google sign-in to get a user id
            FirebaseUser firebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(
                GoogleAuthProvider.GetCredential(
                    (await GoogleSignIn.DefaultInstance.SignIn()).IdToken, null));
            userId = firebaseUser.UserId;
#endif

            // User is not registered if they don't exists in database
            DataSnapshot userSnapshot = await Database.Child("meta/user" + userId).GetValueAsync();
            if (!userSnapshot.Exists)
                return LoadState.NONE;
            else
            {
                //Init user meta
                Dictionary<string, object> userMeta = userSnapshot.Value as Dictionary<string, object>;
                User = new User(userId, new User.UserMeta { characterId = userMeta["characterId"] as string });

                //Init character meta
                DataSnapshot characterSnapshot = await Database.Child("meta/character" + User.Meta.characterId).GetValueAsync();
                Dictionary<string, object> characterMeta = characterSnapshot.Value as Dictionary<string, object>;
                Character = new Character(characterSnapshot.Key, new Character.CharacterMeta
                {
                    firstName = (string)characterMeta["firstName"],
                    lastName = (string)characterMeta["lastName"],
                    prefix = (Character.CharacterPrefix)(long)characterMeta["prefix"]
                });

                return LoadState.CHARACTER;
            }
        }
    }
}