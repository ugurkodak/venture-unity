//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Venture.Data
//{
//	public class Continent
//	{
//		private const int min_size = 4, max_size = 10;

//		public MapAreaInfo Info { get; set; }
//		public List<Region> Regions { get; set; }

//		public Continent()
//		{
//			Regions = new List<Region>();
//		}

//		public Continent Create(int x = 0, int z = 0,
//			int minSize = min_size, int maxSize = max_size,
//			string name = "Unnamed Continent")
//		{
//			Regions = new List<Region>();
//			Info = new MapAreaInfo(x, z, Mathf.Clamp(
//				(int)Mathf.Round(Random.Range(minSize, maxSize)),
//				min_size, max_size), name);

//			Region regionZero = new Region().Create(x, z);
//			Regions.Add(regionZero);
//			for (int i = 1; i < Info.size; i++)
//			{
//				Region region = new Region().Create();
//				region.Info = new MapAreaInfo(i * 2, region.Info.z, region.Info.size, region.Info.name);
//				Regions.Add(region);

//				//Debug.Log("Previous: " + (Regions[i - 1].GetWidth() / 2));
//				//Debug.Log("Current: " + (region.GetWidth() / 2));
//				//region.Info = new MapAreaInfo((Regions[i - 1].GetWidth() / 2) + region.GetWidth() / 2,
//				//	region.Info.z, region.Info.size ,region.Info.name);
//				//Regions.Add(region);
//			}

//			return this;
//		}

//		public Vector3 GetPivot()
//		{
//			int tileCount = 0;
//			Vector3 pivot = new Vector3();
//			foreach (Region region in Regions)
//			{
//				foreach (Tile tile in region.Tiles)
//				{
//					pivot += new Vector3(tile.X, 0, tile.Z);
//					tileCount++;
//				}
//			}
//			return pivot / tileCount;
//		}
//	}
//}

//////Generate regions spirrally
////bool swap = false;
////int direction = 1;
////int steps = 1;
////int remaininSteps = 1;
////int turn = 2;

////			for (int i = 0; i<Info.size; i++)
////			{
////				Region region = new Region().Create(x, z);
////Regions.Add(region);

////				if (swap)
////					x = x + direction * region.GetWidth();
////				else
////					z = z + direction* region.GetHeight();
////remaininSteps--;

////				if (remaininSteps == 0)
////				{
////					swap = !swap;
////					remaininSteps = steps;
////					turn--;
////				}

////				if (turn == 0)
////				{
////					direction = -direction;
////					steps++;
////					remaininSteps = steps;
////					turn = 2;
////				}
////			}