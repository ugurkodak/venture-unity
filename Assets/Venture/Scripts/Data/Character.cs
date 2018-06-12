using System;
using System.Threading.Tasks;

namespace Venture.Data
{
	public class Character : Node
	{
		public string DateCreated;
		public string FirstName;
		public string LastName;
		public string WorldId;
		public string UserId;

		public async Task Create(string firstName, string lastName, string worldId, string userId)
		{
			Document = Collection.Push();
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			FirstName = firstName;
			LastName = lastName;
			WorldId = worldId;
			UserId = userId;
			await Update();
		}
	}
}