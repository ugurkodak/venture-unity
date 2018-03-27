using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public string Name;
	public string World;

	public Player()
	{
		
	}

	public Player(string name, string world)
	{
		Name = name;
		World = world;
	}

	public void CreateNewPlayerData(string id)
	{		
		VentureManager.Database.Child("players").Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWith(task => 
		{
			if (task.IsCompleted)
				Debug.Log("Created new user");
		});
	}
}
