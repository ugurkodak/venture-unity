using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture.Data
{
	public class Region : Node
	{
		//TODO: Remove temporary database placeholders.
		private List<string> regionNames = new List<string> {
			"Efleutan", "Fastairia", "Ugria", "Judrein", "Broylor",
			"Skiytho", "Ufra", "Ablal", "Bleuc Flines", "Sneow Spein" };

		public List<Tile> Tiles;
		public string Name;
		public string CityKey;
		//TODO: public Resource[] Resources;

		public Region()
		{
			Tiles = new List<Tile>();
		}

		public async Task Create(string worldKey)
		{
			Document = Collection.Child(worldKey).Push();
			Name = regionNames[Random.Range(0, regionNames.Count - 1)];
			await Update();
		}

		public Vector3 GetPivot()
		{
			Vector3 pivot = new Vector3();
			foreach (Tile tile in Tiles)
				pivot += new Vector3(tile.X, 0, tile.Z);
			return pivot / Tiles.Count;
		}
	}
}