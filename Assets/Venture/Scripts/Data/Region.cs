using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	[System.Serializable]
	public class Region
	{
		//TODO: Remove temporary database placeholders.
		private List<string> regionNames = new List<string> {
			"Efleutan", "Fastairia", "Ugria", "Judrein", "Broylor",
			"Skiytho", "Ufra", "Ablal", "Bleuc Flines", "Sneow Spein" };

		[SerializeField]
		private string name;
		public string Name { get { return name; } private set { name = value; } }
		[SerializeField]
		private Tile[] tiles;
		public Tile[] Tiles { get { return tiles; } set { tiles = value; } }
		//public bool IsCity { get; private set; }
		//public Resource[] Resources { get; private set; }

		public Region(int size)
		{
			//TODO: Remove temporary database placeholders.
			Name = regionNames[Random.Range(0, regionNames.Count - 1)];
			Tiles = new Tile[size];
		}

		public Vector3 GetPivot()
		{
			Vector3 pivot = new Vector3();
			foreach (Tile tile in Tiles)
				pivot += new Vector3(tile.X, 0, tile.Z);
			return pivot / Tiles.Length;
		}
	}
}