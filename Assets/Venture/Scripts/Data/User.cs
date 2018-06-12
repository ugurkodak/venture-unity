using System;
using System.Threading.Tasks;

namespace Venture.Data
{
	/*NOTES
	- User can only have one active character but abandoned 
	characters persist in simulation until the world end date.
	- User key is set with firebase userid 
	*/
	public class User : Node
	{
		public string DateCreated;
		public string ActiveCharacterKey;
		public string LastSignIn;

		public async Task Create(string id)
		{
			Document = Collection.Child(id);
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			await Update();
		}
	}
}