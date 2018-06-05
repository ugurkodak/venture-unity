using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Venture.Data
{
	public class Character
	{
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string WorldId { get; private set; }
		public string UserId { get; private set; }
		public bool Active { get; private set; }

		public Character Create(string firstName, string lastName, string worldId, string userId)
		{
			FirstName = firstName;
			LastName = lastName;
			WorldId = worldId;
			UserId = userId;
			return this;
		}

		public void Write()
		{
			Access.Root.Child("characters").Push()
			.SetRawJsonValueAsync(JsonConvert.SerializeObject(this))
			.ContinueWith(character => 
			{
				if (character.IsCompleted && character.Exception == null)
					Debug.Log("Success: New character.");
				else
					Debug.Log("Failed: Couldn't add new character.");
			});
		}
		public void Read(string characterId)
		{
			Debug.Log(characterId);
			Access.Root.Child("characters/" + characterId)
			.GetValueAsync().ContinueWith(character => 
			{
				if (character.IsCompleted && character.Exception == null)
				{
					FirstName = character.Result.Child("FirstName").ToString();
					LastName = character.Result.Child("LastName").ToString();
					WorldId = character.Result.Child("WorldId").ToString();
					UserId = character.Result.Child("UserId").ToString();
					Debug.Log(FirstName);
				}
				else
					Debug.Log("Failed: Couldn't read character.");
			});
		}

		//Reads the active character of the user (A user can have only one active character).
		public void ReadActive(string userId)
		{

		}

	}
}
