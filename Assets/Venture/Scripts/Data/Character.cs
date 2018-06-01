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
		//TODO: public bool Active { get; private set; } //What happens if user leaves the world?

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
			Access.Root.Child("characters/" + characterId)
			.GetValueAsync().ContinueWith(character => 
			{
				if (character.IsCompleted && character.Exception == null)
				{
					FirstName = character.Result.Child("FirstName").ToString();
					FirstName = character.Result.Child("LastName").ToString();
					FirstName = character.Result.Child("WorldId").ToString();
					FirstName = character.Result.Child("UserId").ToString();
				}
				else
					Debug.Log("Failed: Couldn't read character.");
			});
		}

	}
}
