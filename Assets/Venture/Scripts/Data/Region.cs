using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class Region
	{
		private const int min_size = 4, max_size = 10;

		public List<Tile> Tiles { get; private set; }

		private MapAreaInfo info;
		public MapAreaInfo Info
		{
			get { return info; }
			set
			{
				foreach (Tile tile in Tiles)
				{
					//Debug.Log("Before: " + tile.x);
					tile.Set(value.x - info.x + tile.x, +value.z - info.z + tile.z,
						tile.direction, tile.sprite);
					//Debug.Log("After: " + tile.x);
				}
				info = value;
			}
		}

		public Region()
		{
			Tiles = new List<Tile>();
		}

		public Region Create(int x = 0, int z = 0,
			int minSize = min_size, int maxSize = max_size, 
			string name = "Unnamed Region")
		{
			Tiles = new List<Tile>();
			info = new MapAreaInfo(x, z, Mathf.Clamp(
				(int)Mathf.Round(Random.Range(minSize, maxSize)),
				min_size, max_size), name);

			//Generate tile positions spirrally
			//floor((n^2)/4) makes a square or neat rectangle
			int tileCount = (int)Mathf.Floor(info.size * info.size * 0.25f);
			bool swap = false;
			int direction = 1;
			int steps = 1;
			int remaininSteps = 1;
			int turn = 2;
			
			//Add tile 0 before looping
			Tile tile = new Tile(x, z, Direction.North, TileSprite.Land);
			Tiles.Add(tile);

			for (int i = 1; i < tileCount; i++)
			{
				if (swap)
					x = x + direction;
				else
					z = z + direction;
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

				tile.Set(x, z, Direction.North, TileSprite.Land);
				Tiles.Add(tile);
			}

			return this;
		}

		public int GetWidth()
		{
			int xMax = Tiles[0].x;
			int xMin = xMax;
			foreach (Tile tile in Tiles)
			{
				int x = tile.x;
				if (x < xMin)
					xMin = x;
				if (x > xMax)
					xMax = x;
			}
			return xMax - xMin + 1;
		}
		public int GetHeight()
		{
			int zMax = Tiles[0].z;
			int zMin = zMax;
			foreach (Tile tile in Tiles)
			{
				int z = tile.z;
				if (z < zMin)
					zMin = z;
				if (z > zMax)
					zMax = z;
			}
			return zMax - zMin + 1;
		}

		public Vector3 GetPivot()
		{
			Vector3 pivot = new Vector3();
			foreach (Tile tile in Tiles)
				pivot += new Vector3(tile.x, 0, tile.z);
			return pivot / Tiles.Count;
		}
	}
}
