using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

//TODO: Error handling
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

	public abstract class DBModel
	{
		[JsonIgnore]
		public DatabaseReference Collection;
		[JsonIgnore]
		public DatabaseReference Reference;
		[JsonIgnore]
		public string Key
		{
			get { return Reference.Key; }
			set { Reference = Collection.Child(value); }
		}

		public DBModel(string parentKeys)
		{
			Collection = Access.Root.Child(GetType().Name + "/" + parentKeys);
			Reference = Collection.Push();
		}

		public async Task Load(string key)
		{
			Reference = Collection.Child(key);
			DataSnapshot snapshot = await Reference.GetValueAsync();
			if (snapshot.Exists)
				JsonConvert.PopulateObject(snapshot.GetRawJsonValue(), this);
		}

		public async Task Update()
		{
			await Reference.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public async Task Delete()
		{
			await Reference.RemoveValueAsync();
			Key = null;
			Reference = null;
		}
	}

	public class DBList<T> : List<T> where T : DBModel
	{
		private DatabaseReference reference;

		public DBList(string parentKeys)
		{
			reference = Access.Root.Child(typeof(T).Name + "/" + parentKeys);
		}

		public async Task Update()
		{
			Dictionary<string, object> collection = new Dictionary<string, object>();
			foreach (T model in this)
				collection.Add(model.Key, model);
			await reference.SetRawJsonValueAsync(JsonConvert.SerializeObject(collection));
		}

		public async Task Delete()
		{
			await reference.RemoveValueAsync();
			reference = null;
		}
	}
}