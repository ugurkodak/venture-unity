using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class World
	{
		public const int Width = 40, Height = 20;
		const int RegionWidth = 4, RegionHeight = 4;
		const float frequency = 0.06f, landAmount = 0.4f;

		public string Name { get; private set; }
		public Tile[,] Tiles { get; private set; }

		public World()
		{
			Tiles = new Tile[Height, Width];
		}

		public World Create()
		{
			//Generate map
			float randomSeed = Random.value * 100;
			for (int z = 0; z < Height; z++)
			{
				for (int x = 0; x < Width ; x++)
				{
					float value = Mathf.PerlinNoise(frequency * (x - 0.5f) + randomSeed, 
						frequency * (z - 0.5f) + randomSeed);
					if (value < landAmount)
					{
						Tiles[z, x] = new Tile(x, z, Direction.North, Tile.TileSprite.Land);
					}
					else
					{
						Tiles[z, x] = new Tile(x, z, Direction.North, Tile.TileSprite.Water);
					}
				}
			}

			////Define regions
			int regionCountX = Width / RegionWidth;
			int regionCountZ = Height / RegionHeight;

			for (int i = 0; i < regionCountZ; i++)
			{
				for (int j = 0; j < regionCountX; j++)
				{
					for (int z = i * RegionHeight; z < i * RegionHeight + RegionHeight; z++)
					{
						for (int x = j * RegionWidth; x < j * RegionWidth + RegionWidth; x++)
						{
							Debug.Log("Iterated");
							if (Tiles[z, x].Sprite == Tile.TileSprite.Land)
							{
								//TODO: Add to regions
								Debug.Log("Land Found");
								Tiles[z, x].Sprite = Tile.TileSprite.Empty;
							}
						}
					}
				}
			}
			return this;
		}
	}
}
