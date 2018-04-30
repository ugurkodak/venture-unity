using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

namespace Venture.Data
{
	[Serializable]
	public class World
	{
		//TODO: Remove temporary database placeholders.
		private List<string> worldNames = new List<string> { "Iptos", "Adarr", "Ixdar", "Lius" };

		//World area needs must be perfectly divisible by region area
		//TODO: Maybe make it configurible
		public const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		[Serializable]
		public class WorldMeta
		{
			[SerializeField]
			private string name;
			[SerializeField]
			private long startDate, endDate; //Converted to DateTime via property to be able to serialize
			[SerializeField]
			private int characterCount;
			//TODO: private int cityCount;
			//TODO: private bool active;

			public string Name { get { return name; } set { name = value; } }
			public DateTime StartDate
			{
				get { return DateTime.FromFileTimeUtc(startDate); }
				set { startDate = value.ToFileTimeUtc(); }
			}
			public DateTime EndDate
			{
				get { return DateTime.FromFileTimeUtc(endDate); }
				set { endDate = value.ToFileTimeUtc(); }
			}
			public int CharacterCount { get { return characterCount; } set { characterCount = value; } }

			public WorldMeta(string name, int hours)
			{
				Name = name;
				StartDate = DateTime.Now;
				EndDate = StartDate.AddHours(hours);
				CharacterCount = 0;
			}
		}

		[SerializeField]
		private WorldMeta meta;
		[SerializeField]
		private Region[] regions;
		[SerializeField]
		private Tile[] oceans;

		public WorldMeta Meta { get { return meta; } private set { meta = value; } }
		//TODO: Check if using arrays can cause problem
		public Region[] Regions { get { return regions; } private set { regions = value; } }
		public Tile[] Oceans { get { return oceans; } private set { oceans = value; } }

		public World Create()
		{
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
			List<Region> regionFound = new List<Region>();
			List<Tile> oceanTiles = new List<Tile>();
			for (int i = 0; i < regionCountZ; i++)
				for (int j = 0; j < regionCountX; j++)
				{
					List<Tile> regionTiles = new List<Tile>();
					for (int z = i * RegionHeight; z < i * RegionHeight + RegionHeight; z++)
						for (int x = j * RegionWidth; x < j * RegionWidth + RegionWidth; x++)
							if (Tiles[z, x].Sprite == Tile.TileSprite.Land)
								regionTiles.Add(Tiles[z, x]);
							else if (Tiles[z, x].Sprite == Tile.TileSprite.Water)
								oceanTiles.Add(Tiles[z, x]);
					if (regionTiles.Count > 0)
						regionFound.Add(new Region(regionTiles.Count) { Tiles = regionTiles.ToArray() });
				}
			Regions = regionFound.ToArray();
			Oceans = oceanTiles.ToArray();
			//TODO: Remove temporary database placeholders.
			Meta = new WorldMeta(worldNames[UnityEngine.Random.Range(0, worldNames.Count)], 5);
			return this;
		}

		public void Read(string Id)
		{
			//TODO: Split tile data to seperate document. (Maybe not?)
			//Access.Root.Child("worlds").Child(Id + "/meta").
		}

		public void Write()
		{
			//TODO: Split tile data to seperate document
			Access.Root.Child("worlds").Push().SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWith(task =>
			{
				if (task.IsCompleted)
					Debug.Log("New world saved.");
				else
					Debug.Log("Problem occured while saving the world.");
			});
		}
	}
}
