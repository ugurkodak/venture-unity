using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture.Data
{
	//TODO: Figure out how to remove the extra List nodes
	public class World
	{
		public class WorldInfo : Node
		{
			//TODO: Remove temporary database placeholders.
			private List<string> worldNames = new List<string> { "Iptos", "Adarr", "Ixdar", "Lius" };

			public string Name;
			public string DateCreated;
			public string EndDate;
			public int CharacterCount;

			//public WorldInfo()
			//{
			//	Collection = Access.Root.Child("World/Info");
			//}

			public async Task Create(int hours)
			{
				Document = Collection.Push();
				DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
				EndDate = DateTime.Now.AddHours(hours).ToString(Access.DATE_TIME_FORMAT);
				Name = worldNames[UnityEngine.Random.Range(0, worldNames.Count)];
				CharacterCount = 0;
				await Update();
			}

			//public async Task<bool> Read(string worldKey)
			//{
			//	Document = Collection.Child(worldKey);
			//	var values = await Document.GetValueAsync();
			//	if (values.Exists)
			//	{
			//		JsonConvert.PopulateObject(values.GetRawJsonValue(), this);
			//		return true;
			//	}
			//	else
			//		return false;
			//}
		}

		public class WorldWaters : Node
		{
			public List<Tile> List;
			public WorldWaters()
			{
				List = new List<Tile>();
			}

			public async Task Create(string worldKey)
			{
				Document = Collection.Child(worldKey);
				await Update();
			}
		}

		public class WorldRegions: Node
		{
			//TODO: Remove temporary database placeholders.
			private List<string> regionNames = new List<string> {
			"Efleutan", "Fastairia", "Ugria", "Judrein", "Broylor",
			"Skiytho", "Ufra", "Ablal", "Bleuc Flines", "Sneow Spein" };

			public List<Region> List;
			public WorldRegions()
			{
				List = new List<Region>();
			}

			public async Task Create(string worldKey)
			{
				foreach (Region region in List)
					region.Name = regionNames[UnityEngine.Random.Range(0, regionNames.Count - 1)];
				Document = Collection.Child(worldKey);
				await Update();
			}
		}

		//World area needs must be perfectly divisible by region area
		//TODO: Maybe make it configurible
		const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		public WorldInfo Info;
		public WorldRegions Regions;
		public WorldWaters Waters;

		public World()
		{
			Info = new WorldInfo();
			Regions = new WorldRegions();
			Waters = new WorldWaters();
		}

		public async Task Create()
		{
			await Info.Create(5); //TODO: How do we decide the world interval?

			//Generate map
			Tile[,] Tiles = new Tile[Height, Width];
			float randomSeed = UnityEngine.Random.value * 100;
			for (int z = 0; z < Height; z++)
				for (int x = 0; x < Width; x++)
				{
					float value = Mathf.PerlinNoise(frequency * (x - 0.5f) + randomSeed,
						frequency * (z - 0.5f) + randomSeed);
					if (value < landAmount)
						Tiles[z, x] = new Tile(x, z, Direction.North, Tile.TileSprite.Land);
					else
						Tiles[z, x] = new Tile(x, z, Direction.North, Tile.TileSprite.Water);
				}

			//Define regions
			int regionCountX = Width / RegionWidth;
			int regionCountZ = Height / RegionHeight;
			for (int i = 0; i < regionCountZ; i++)
				for (int j = 0; j < regionCountX; j++)
				{
					List<Tile> regionTiles = new List<Tile>();
					for (int z = i * RegionHeight; z < i * RegionHeight + RegionHeight; z++)
						for (int x = j * RegionWidth; x < j * RegionWidth + RegionWidth; x++)
							if (Tiles[z, x].Sprite == Tile.TileSprite.Land)
								regionTiles.Add(Tiles[z, x]);
							else if (Tiles[z, x].Sprite == Tile.TileSprite.Water)
								Waters.List.Add(Tiles[z, x]);
					if (regionTiles.Count > 0)
						Regions.List.Add(new Region { Tiles = regionTiles });
				}
			await Waters.Create(Info.Document.Key);
			await Regions.Create(Info.Document.Key);
			//foreach (Region region in Regions)
			//	await region.Create(Info.Document.Key);
		}

		public async Task Read(string key)
		{
			await Info.Read(key);
			await Waters.Read(key);
			await Regions.Read(key);
			//var regionsSnapshot = (await Access.Root.Child("Region").GetValueAsync()).Children;	
			//foreach (Region region in regions.Children as new List<Region>())
			//	Regions.Add(region);
		}
	}
}
