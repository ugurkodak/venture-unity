using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Venture.Data
{
	public class Character : Data
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string WorldId { get; set; }
		public string UserId { get; set; }

		public async Task Create(string firstName, string lastName, string worldId, string userId)
		{
			FirstName = firstName;
			LastName = lastName;
			WorldId = worldId;
			UserId = userId;
			await base.Create();
		}
	}
}
