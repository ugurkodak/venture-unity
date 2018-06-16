using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venture.Data
{
	public class WorldInfo : DBModel
	{
		//TODO: Remove temporary database placeholders.
		private List<string> worldNames = new List<string> { "Iptos", "Adarr", "Ixdar", "Lius" };

		public string Name;
		public string DateCreated;
		public string EndDate;
		public int CharacterCount;

		public WorldInfo() : base(null) { }

		public WorldInfo Create(int hours)
		{
			DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
			EndDate = DateTime.Now.AddHours(hours).ToString(Access.DATE_TIME_FORMAT);
			Name = worldNames[UnityEngine.Random.Range(0, worldNames.Count)];
			CharacterCount = 0;
			return this;
		}
	}
}
