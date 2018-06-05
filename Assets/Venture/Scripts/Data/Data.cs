using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

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
}