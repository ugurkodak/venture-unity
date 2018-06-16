using System;
using System.Threading.Tasks;

namespace Venture.Data
{
	public class Character : DBModel
	{
		public string DateCreated;
		public string FirstName;
		public string LastName;
		public string WorldId;
		public string UserId;

		public Character() : base(null) { }

		public void Create(string firstName, string lastName, string worldId, string userId)
		{
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			FirstName = firstName;
			LastName = lastName;
			WorldId = worldId;
			UserId = userId;
		}
	}
}