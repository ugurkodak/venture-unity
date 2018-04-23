using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class Region
	{
		public Tile[] Tiles { get; set; }
		//public bool IsCity { get; private set; }
		//public Resource[] Resources { get; private set; }

		public Region(int size)
		{
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
