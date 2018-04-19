using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class Continent
	{
		private const int min_size = 4, max_size = 10;

		public MapAreaInfo Info { get; set; }
		public List<Region> Regions { get; set; }

		public Continent()
		{
			Regions = new List<Region>();
		}

		public Continent Create(int x = 0, int z = 0,
			int minSize = min_size, int maxSize = max_size,
			string name = "Unnamed Continent")
		{
			Regions = new List<Region>();
			Info = new MapAreaInfo(x, z, Mathf.Clamp(
				(int)Mathf.Round(Random.Range(minSize, maxSize)),
				min_size, max_size), name);

			bool swap = false;
			int direction = 1;
			int steps = 1;
			int remaininSteps = 1;
			int turn = 2;
			//Generate regions spirrally
			for (int i = 1; i < Info.size; i++)
			{
				Region region = new Region().Create(x, z);

				if (swap)
					x = x + direction * region.GetWidth();
				else
					z = z + direction * region.GetHeight();
				remaininSteps--;

				if (remaininSteps == 0)
				{
					swap = !swap;
					remaininSteps = steps;
					turn--;
				}

				if (turn == 0)
				{
					direction = -direction;
					steps++;
					remaininSteps = steps;
					turn = 2;
				}

				Regions.Add(region);
			}

			//for (int i = 0; i < numberOfRegions; i++)
			//{
			//	Region newRegion = new Region();
			//	newRegion.Create("region" + 1, x, z, 5);
			//	foreach (Region region in regions)
			//	{
			//		regions.FindLast()
			//	}
			//}
			return this;
		}

		public Vector3 GetPivot()
		{
			int tileCount = 0;
			Vector3 pivot = new Vector3();
			foreach (Region region in Regions)
			{
				foreach (Tile tile in region.Tiles)
				{
					pivot += new Vector3(tile.x, 0, tile.z);
					tileCount++;
				}
			}
			return pivot / tileCount;
		}
	}
}