using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

	public class DBModel
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

		public DBModel(params string[] parentKeys)
		{
			string path = "";
			if (parentKeys != null)
				foreach (string key in parentKeys)
					path += "/" + key;
			Collection = Access.Root.Child(GetType().Name + path);
			Reference = Collection.Push();
		}

		public async Task Load(string key)
		{
			Key = key;
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
		private List<string> parentKeys;
		public DatabaseReference Reference;

		public DBList(params string[] parentKeys)
		{
			this.parentKeys = new List<string>();
			string path = "";
			if (parentKeys != null)
				foreach (string key in parentKeys)
				{
					this.parentKeys.Add(key);
					path += "/" + key;
				}
			Reference = Access.Root.Child(typeof(T).Name + path);
		}

		public async Task Update()
		{
			Dictionary<string, object> collection = new Dictionary<string, object>();
			foreach (T model in this)
				collection.Add(model.Key, model);
			await Reference.SetRawJsonValueAsync(JsonConvert.SerializeObject(collection));
		}

		public async Task Load()
		{
			Dictionary<string, object> collection = new Dictionary<string, object>();
			DataSnapshot snapshot = await Reference.GetValueAsync();
			if (snapshot.Exists)
				collection = snapshot.Value as Dictionary<string, object>;

			foreach (var document in collection)
			{
				//Instantiate DBModels with different number of constructors
				T model;
				switch (parentKeys.Count)
				{
					case 0:
						model = (T)Activator.CreateInstance(typeof(T));
						break;
					case 1:
						model = (T)Activator.CreateInstance(typeof(T),
							parentKeys[0]);
						break;
					case 2:
						model = (T)Activator.CreateInstance(typeof(T),
							parentKeys[0], parentKeys[1]);
						break;
					case 3:
						model = (T)Activator.CreateInstance(typeof(T), 
							parentKeys[0], parentKeys[1], parentKeys[2]);
						break;
					case 4:
						model = (T)Activator.CreateInstance(typeof(T), 
							parentKeys[0], parentKeys[1], parentKeys[2], parentKeys[3]);
						break;
					default:
						model = null;
						Debug.LogError("Parent count exceeded nesting limit.");
						break;
				}
				JsonConvert.PopulateObject(JsonConvert.SerializeObject(document.Value), model);
				model.Key = document.Key;
				Add(model);
			}
		}

		public async Task Delete()
		{
			await Reference.RemoveValueAsync();
			Reference = null;
		}

		public T GetItem(string key)
		{
			foreach (T item in this)
				if (item.Key == key)
					return item;
			return null;
		}
	}
}