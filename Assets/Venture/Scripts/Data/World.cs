using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

namespace Venture.Data
{
	[System.Serializable]
	public class World
	{
		//TODO: Remove temporary database placeholders.
		private List<string> worldNames = new List<string> { "Iptos", "Adarr", "Ixdar", "Lius"};

		//World area needs must be perfectly divisible by region area
		public const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		[SerializeField]
		private string name;
		public string Name { get { return name; } private set { name = value; } }
		[SerializeField]
		private Region[] regions;
		public Region[] Regions { get { return regions; } private set { regions = value; } }
		[SerializeField]
		private Tile[] oceans;
		public Tile[] Oceans { get { return oceans; } private set { oceans = value; } }

		public World Create()
		{
			//Generate map
			Tile[,] Tiles = new Tile[Height, Width];
			float randomSeed = Random.value * 100;
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

			////Define regions
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
			Name = worldNames[Random.Range(0, worldNames.Count)];
			return this;
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
