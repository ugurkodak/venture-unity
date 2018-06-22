using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture.Data
{
	/*
	 * - User can only have one active character but abandoned 
	 * characters persist in simulation until the world end date.
	 * To be able to start over, user needs to abandon its current
	 * world and create a new character at another world. User can
	 * not go back to an abandonned world.
	 * - User key is set with firebase userid 
	 */
	public class User : DBModel
	{
		public string DateCreated;
		public string ActiveCharacterKey;
		public string LastSignIn;

		public void Create(string id)
		{
			Key = id;
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			LastSignIn = DateCreated;
		}

		public async Task UpdateLastSignIn()
		{
			LastSignIn = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			await Update();
		}

		public async Task UpdateActiveCharacter(string characterId)
		{
			ActiveCharacterKey = characterId;
			await Update();
		}
	}
}