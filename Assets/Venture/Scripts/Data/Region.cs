using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
	public class Region
	{
		private const int min_size = 4, max_size = 10;

		private int x, z, size;
		private string name;
		private List<Tile> tiles = new List<Tile>();

		public Region() { }

		public Region Create(int x = 0, int z = 0, int minSize = min_size, int maxSize = max_size, string name = "New Region")
		{
			this.name = name;
			this.x = x;
			this.z = z;
			size = (int)Mathf.Round(Random.Range(minSize, maxSize));
			size = Mathf.Clamp(size, min_size, max_size);

			//Generate tile positions spirrally
			//floor((n^2)/4) makes a square or neat rectangle
			int tileCount = (int)Mathf.Floor(size * size * 0.25f);
			bool swap = false;
			int direction = 1;
			int steps = 1;
			int remaininSteps = 1;
			int turn = 2;

			Tile tileZero = new Tile(x, z);
			tiles.Add(tileZero);

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

				tiles.Add(new Tile(x, z));
			}

			return this;
		}

		public string GetName()
		{
			return name;
		}

		public List<Tile> GetTiles()
		{
			return tiles;
		}

		public int GetWidth()
		{
			int xMax = (int)GetTiles()[0].GetPosition().x;
			int xMin = xMax;
			foreach (Tile i in GetTiles())
			{
				int x = (int)i.GetPosition().x;
				if (x < xMin)
					xMin = x;
				if (x > xMax)
					xMax = x;
			}
			return xMax - xMin + 1;
		}
		public int GetHeight()
		{
			int zMax = (int)GetTiles()[0].GetPosition().z;
			int zMin = zMax;
			foreach (Tile i in GetTiles())
			{
				int z = (int)i.GetPosition().z;
				if (z < zMin)
					zMin = z;
				if (z > zMax)
					zMax = z;
			}
			return zMax - zMin + 1;
		}

		public Vector3 GetAbsoluteCenter()
		{
			Vector3 center = new Vector3();
			foreach (Tile tile in tiles)
				center += tile.GetPosition();
			return center / tiles.Count;
		}
	}
}
