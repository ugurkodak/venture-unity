using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture.Data
{
	//TODO: Figure out how to remove the extra List nodes
	public class World
	{
		//World area needs must be perfectly divisible by region area
		//TODO: Maybe make it configurible
		const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		public WorldInfo Info;
		public DBList<OceanTile> Waters;
		public List<Region> Regions;

		public World()
		{
			Info = new WorldInfo();
			Regions = new List<Region>();
		}

		public void Create()
		{
			Info.Create(5); //TODO: How do we decide the world interval?
			Waters = new DBList<OceanTile>(Info.Key);

			//Generate map
			string[,] Tiles = new string[Height, Width];
			float randomSeed = UnityEngine.Random.value * 100;
			for (int z = 0; z < Height; z++)
				for (int x = 0; x < Width; x++)
				{
					float value = Mathf.PerlinNoise(frequency * (x - 0.5f) + randomSeed,
						frequency * (z - 0.5f) + randomSeed);
					if (value < landAmount)
						Tiles[z, x] = "land";
					else
						Tiles[z, x] = "water";
				}

			//Define regions
			int regionCountX = Width / RegionWidth;
			int regionCountZ = Height / RegionHeight;
			for (int i = 0; i < regionCountZ; i++)
				for (int j = 0; j < regionCountX; j++)
				{
					Region region = new Region(Info.Key);
					DBList<RegionTile> regionTiles = new DBList<RegionTile>(Info.Key + "/" + region.Info.Key);
					for (int z = i * RegionHeight; z < i * RegionHeight + RegionHeight; z++)
						for (int x = j * RegionWidth; x < j * RegionWidth + RegionWidth; x++)
							if (Tiles[z, x] == "land")
							{
								RegionTile regionTile = new RegionTile(Info.Key, region.Info.Key);
								regionTile.Create(x, z);
								regionTiles.Add(regionTile);

							}
							else if (Tiles[z, x] == "water")
							{
								OceanTile oceanTile = new OceanTile(Info.Key);
								oceanTile.Create(x, z);
								Waters.Add(oceanTile);
							}
					if (regionTiles.Count > 0)
					{
						region.Create(regionTiles);
						Regions.Add(region);
					}
				}
		}

		public async Task Update()
		{
			await Info.Update();
			await Waters.Update();
			foreach (Region region in Regions)
				await region.Update();
			Debug.Log("World updated.");
		}

		public async Task Delete()
		{
			await Info.Delete();
			await Waters.Delete();
			foreach (Region region in Regions)
				await region.Delete();
			Debug.Log("World deleted.");
		}

		//public async Task Add()
		//{
		//	//await Info.Add();
		//	//Regions.Path = "World/Region/" + Info.Key;
		//	//Waters.Path = "World/Waters/" + Info.Key;
		//	//await Regions.Add();
		//	//await Waters.Add();
		//	//Debug.Log("World added");
		//}

		//public async Task Load(string key)
		//{
		//	//await Info.Load(key);
		//	//await Regions.Load("World/Regions/" + Info.Key);
		//	////await Waters.Load("World/Waters/" + Info.Key);
		//	//Debug.Log("Done");
		//}
	}
}
