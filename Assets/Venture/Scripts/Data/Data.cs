using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using System;

namespace Venture.Data
{
	public enum Direction { North, East, South, West };
	public enum Resource { Agriculture, Mining};

	public static class Access
	{
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

	//public struct MapAreaInfo
	//{
	//	public int x, z, size;
	//	public string name;

	//	public MapAreaInfo(int x, int z, int size, string name)
	//	{
	//		this.x = x;
	//		this.z = z;
	//		this.size = size;
	//		this.name = name;
	//	}
	//}
}