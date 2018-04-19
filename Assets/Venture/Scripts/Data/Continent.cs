using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class Continent
	{
		string name;
		int x, z, size;
		List<Region> regions = new List<Region>();

		public void Create(string name, int size, int x, int z)
		{
			this.name = name;
			this.x = x;
			this.z = z;
			this.size = size;

			bool swap = false;
			int direction = 1;
			int steps = 1;
			int remaininSteps = 1;
			int turn = 2;

			//Generate regions spirrally
			for (int i = 1; i < size; i++)
			{
				Region region = new Region().Create();

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

				regions.Add(region);
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
		}

		public List<Region> GetRegions()
		{
			return regions;
		}
	}
}