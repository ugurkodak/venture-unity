using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Exception handling
namespace Venture.Data
{
	public enum Direction { North, East, South, West };
	public enum Resource { Agriculture, Mining};

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

	//TODO: Exception handling
	public abstract class Data
	{
		protected DatabaseReference collection;
		protected DatabaseReference document;

		public Data()
		{
			collection = Access.Root.Child(GetType().Name);
		}

		public virtual async Task Create()
		{
			document = collection.Push();
			await document.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public virtual async Task Update()
		{
			await document.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		}

		public virtual async Task Read()
		{
			var values = await document.GetValueAsync();
			JsonConvert.PopulateObject(values.GetRawJsonValue(), this);
		}

		public virtual async Task Delete()
		{
			await document.RemoveValueAsync();
		}

	}

	public class User : Data
	{
		private readonly string id;
		public string JoinDate { get; set; }
		//User can only have one active character but abandoned 
		//characters persist in simulation until the world end date
		public string ActiveCharacterId { get; set; }
		public string LastLogin { get; set; }
		//TODO: User stats

		public User(string id)
		{
			this.id = id;
		}

		//User has a special case for create. It's id should be manualy set
		public override async Task Create()
		{
			JoinDate = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			document = collection.Child(id);
			await Update();
			//var user = await Access.Root.Child("users/" + id).GetValueAsync();
			//if (!user.Exists)
			//{
				
			//	await Update();
			//	Debug.Log("New user created.");
			//}
			//else
			//	Debug.LogError("User already exists.");
		}

		//public async Task Update()
		//{
		//	await Access.Root.Child("users/" + id)
		//	.SetRawJsonValueAsync(JsonConvert.SerializeObject(this));
		//}

		//public async Task<bool> Read()
		//{
		//	var user = await Access.Root.Child("users/" + id).GetValueAsync();
		//	if (user.Exists)
		//	{
		//		ActiveCharacterId = user.Child("ActiveCharacterId").GetRawJsonValue();
		//		JoinDate = user.Child("JoinDate").GetRawJsonValue();
		//		LastLogin = user.Child("LastLogin").GetRawJsonValue();
		//		return true;
		//	}
		//	else
		//	{
		//		Debug.LogError("Couldn't read user.");
		//		return false;
		//	}
		//}

		////public async Task Delete()
		////{

		////}

		//public async Task UpdateActiveCharacter(string characterId)
		//{
		//	ActiveCharacterId = characterId;
		//	await Update();
		//}

		//public async Task UpdateLastLogin()
		//{
		//	LastLogin = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
		//	await Update();
		//}
	}
}