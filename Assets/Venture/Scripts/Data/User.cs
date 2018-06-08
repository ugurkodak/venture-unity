using System;
using System.Threading.Tasks;

namespace Venture.Data
{
	/*NOTES
	- User can only have one active character but abandoned 
	characters persist in simulation until the world end date.
	- User key is set with firebase userid 
	*/
	public class User : Data
	{
		public string DateCreated { get; set; }
		public string ActiveCharacterId { get; set; }
		public string LastSignIn { get; set; }

		public async Task Create(string id)
		{
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			Document = Collection.Child(id);
			await base.Update();
		}
	}
}
