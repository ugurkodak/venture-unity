using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using Newtonsoft.Json;

namespace Venture.Data
{
	public class World
	{
		//TODO: Remove temporary database placeholders.
		private List<string> worldNames = new List<string> { "Iptos", "Adarr", "Ixdar", "Lius" };

		//World area needs must be perfectly divisible by region area
		//TODO: Maybe make it configurible
		const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		public class WorldMeta
		{
			public string Name { get; private set; }
			public DateTime StartDate { get; private set; }
			public DateTime EndDate { get; private set; }
			public int CharacterCount { get; set; }
			//TODO: private int cityCount;
			//TODO: private bool active;

			public WorldMeta(string name, int hours)
			{
				Name = name;
				StartDate = DateTime.Now;
				EndDate = StartDate.AddHours(hours);
				CharacterCount = 0;
			}
		}

		public WorldMeta Meta { get; private set; }
		//TODO: Check if using arrays can cause problem
		public Region[] Regions { get; private set; }
		public Tile[] Oceans { get; private set; }

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
			Access.Root.Child("worlds/" + Id).GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					DataSnapshot snapshot = task.Result;
					Debug.Log(snapshot.Value);
				}
				else
					Debug.Log("Problem occured while reading the world.");
			});
		}

		public void Write()
		{
			string id = Access.Root.Child("worlds/meta").Push().Key;
			Access.Root.Child("worlds/meta/" + id)
			.SetRawJsonValueAsync(JsonConvert.SerializeObject(Meta))
			.ContinueWith(meta =>
			{
				if (meta.IsCompleted && meta.Exception == null)
				{
					Debug.Log("Success: World meta wrote.");
					Access.Root.Child("worlds/raw/" + id + "/regions")
					.SetRawJsonValueAsync(JsonConvert.SerializeObject(Regions))
					.ContinueWith(regions =>
					{
						if (regions.IsCompleted && regions.Exception == null)
						{
							Debug.Log("Success: World regions wrote.");
							Access.Root.Child("worlds/raw/" + id + "/oceans")
							.SetRawJsonValueAsync(JsonConvert.SerializeObject(Oceans))
							.ContinueWith(oceans =>
							{
								if (oceans.IsCompleted && oceans.Exception == null)
									Debug.Log("Success: World oceans wrote.");
								else
									Debug.Log(oceans.Exception);
							});
						}
						else
							Debug.Log(regions.Exception);
					});
				}
				else
					Debug.Log("META: " + meta.Exception);
			});
		}
	}
}
