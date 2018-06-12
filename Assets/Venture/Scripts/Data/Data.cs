using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System.Threading.Tasks;

//TODO: Exception handling
//TODO: CRUD functions stop excecuting when editor window is not focused
namespace Venture.Data
{
	public enum Direction { North, East, South, West };

	public static class Access
	{
		public const string DATE_TIME_FORMAT = "yyyymmddhhmm";
		public static DatabaseReference Root;

		static Access()
		{
			//UNITY_EDITOR User
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://venture-196117.firebaseio.com/");
			FirebaseApp.DefaultInstance.SetEditorP12FileName("Venture-9af379c14c56.p12");
			FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("venture-196117@appspot.gserviceaccount.com");
			FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
			Root = FirebaseDatabase.DefaultInstance.RootReference;
		}
	}

	public abstract class Node
	{
		[JsonIgnore]
		public DatabaseReference Collection;
		[JsonIgnore]
		public DatabaseReference Document;

		public Node()
		{
			Collection = Access.Root.Child(GetType().Name);
		}

		public async Task<bool> Read(string key)
		{
			Document = Collection.Child(key);
			var values = await Document.GetValueAsync();
			if (values.Exists)
			{
				JsonConvert.PopulateObject(values.GetRawJsonValue(), this);
				return true;
			}
			else
				return false;
		}

		public async Task Update()
		{
			await Document.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public async Task Delete()
		{
			await Document.RemoveValueAsync();
		}
	}
}