using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Venture.Data
{
	//TODO: Figure out how to remove the extra List nodes
	public class World
	{
		//World area needs must be perfectly divisible by region area
		//TODO: Maybe make it configurible?
		const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		public WorldInfo Info;
		public DBList<OceanTile> OceanTiles;
		public DBList<RegionInfo> RegionInfos;
		public List<DBList<RegionTile>> RegionTiles;
		//TODO: public DBList<RegionResource> Resources;

		public World()
		{
			Info = new WorldInfo();
			RegionTiles = new List<DBList<RegionTile>>();
		}

		public void Create()
		{
			Info.Create(5); //TODO: How do we decide the world interval?
			RegionInfos = new DBList<RegionInfo>(Info.Key);
			OceanTiles = new DBList<OceanTile>(Info.Key);

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
					RegionInfo regionInfo = new RegionInfo(Info.Key);
					DBList<RegionTile> regionTiles = new DBList<RegionTile>(Info.Key, regionInfo.Key);
					for (int z = i * RegionHeight; z < i * RegionHeight + RegionHeight; z++)
						for (int x = j * RegionWidth; x < j * RegionWidth + RegionWidth; x++)
							if (Tiles[z, x] == "land")
							{
								RegionTile regionTile = new RegionTile(Info.Key, regionInfo.Key);
								regionTile.Create(x, z);
								regionTiles.Add(regionTile);

							}
							else if (Tiles[z, x] == "water")
							{
								OceanTile oceanTile = new OceanTile(Info.Key);
								oceanTile.Create(x, z);
								OceanTiles.Add(oceanTile);
							}
					if (regionTiles.Count > 0)
					{
						regionInfo.Create();
						RegionInfos.Add(regionInfo);
						RegionTiles.Add(regionTiles);
					}
				}
		}

		public async Task Update()
		{
			await Info.Update();
			await OceanTiles.Update();
			await RegionInfos.Update();
			foreach (DBList<RegionTile> regionTiles in RegionTiles)
				await regionTiles.Update();
			Debug.Log("World updated.");
		}

		public async Task Load(string key)
		{
			await Info.Load(key);
			OceanTiles = new DBList<OceanTile>(Info.Key);
			await OceanTiles.Load();
			RegionInfos = new DBList<RegionInfo>(Info.Key);
			await RegionInfos.Load();
			foreach (RegionInfo regionInfo in RegionInfos)
			{
				DBList<RegionTile> regionTiles = new DBList<RegionTile>(Info.Key, regionInfo.Key);
				await regionTiles.Load();
				RegionTiles.Add(regionTiles);
			}
			Debug.Log("World loaded.");
		}

		public async Task Delete()
		{
			await Info.Delete();
			await OceanTiles.Delete();
			await RegionInfos.Delete();
			foreach (DBList<RegionTile> regionTiles in RegionTiles)
				await regionTiles.Delete();
			Debug.Log("World deleted.");
		}
	}
}
