using System.Collections.Generic;

namespace Venture.Data
{
	public class RegionInfo : DBModel
	{
		//TODO: Remove temporary database placeholders.
		private List<string> regionNames = new List<string> {
			"Efleutan", "Fastairia", "Ugria", "Judrein", "Broylor",
			"Skiytho", "Ufra", "Ablal", "Bleuc Flines", "Sneow Spein" };

		public string Name;
		public string CityKey;

		public RegionInfo(string worldKey) : base(worldKey) { }

		public void Create()
		{
			Name = regionNames[UnityEngine.Random.Range(0, regionNames.Count - 1)];
		}
	}
}
