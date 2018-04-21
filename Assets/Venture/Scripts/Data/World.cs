using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class World
	{
		const int width = 160, height = 80;
		string name;
		public List<Tile> Tiles { get; private set; }
		public List<Tile> Regions { get; private set; }

		public World()
		{
			Tiles = new List<Tile>();
			Regions = new List<Tile>();
		}

		public World Create(int width = width, int height = height,
			float seed = 0, float frequency = 0.06f, float landAmount = 0.4f)
		{
			float randomSeed = Random.value * 100;
			if (seed != 0)
				randomSeed = seed;
			for (int z = 0; z < height; z++)
			{
				for (int x = 0; x < width ; x++)
				{
					float value = Mathf.PerlinNoise(frequency * (x - 0.5f) + randomSeed, frequency * (z - 0.5f) + randomSeed);
					if (value < landAmount)
						Tiles.Add(new Tile(x, z, Direction.North, TileSprite.Land));
					else
						Tiles.Add(new Tile(x, z, Direction.North, TileSprite.Water));
				}
			}
			return this;
		}
	}
}
