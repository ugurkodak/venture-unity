using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

namespace Venture.Data
{
	public enum Direction { North, East, South, West };
	public enum TileSprite { Empty, Water, Land };

	public class Tile
	{
		int x, z;
		Direction direction;
		TileSprite sprite;

		public Tile(int x, int z, 
			TileSprite sprite = TileSprite.Water, 
			Direction direction = Direction.North)
		{
			this.x = x;
			this.z = z;
			this.sprite = sprite;
			this.direction = direction;
		}

		public void Write(DatabaseReference reference)
		{
			reference.Child(reference.Push().Key)
				.SetRawJsonValueAsync(JsonUtility.ToJson(this))
				.ContinueWith(task => 
			{
				if (task.IsCompleted)
					Debug.Log("Tile added.");
				else
					Debug.Log("Problem adding tile");
			});
		}

		public Vector3 GetPosition()
		{
			return new Vector3(x, 0, z);
		}

		public TileSprite GetSprite()
		{
			return sprite;
		}
	}

	public class Region
	{
		string name;
		int x, z, size;
		List<Tile> tiles = new List<Tile>();

		public Region()
		{
		}

		public void Create(string name, int x, int z, int size)
		{
			this.name = name;
			this.x = x;
			this.z = z;
			this.size = size;

			//Generate tile positions spirrally expanding from 0,0,0
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
	}

	//Key >> MapId
	public class Continent
	{
		string name;
	}

	public class World
	{
		string name;
		int width;
		int height;
		List<Tile> tiles = new List<Tile>();
		List<Region> regions = new List<Region>();
		List<Continent> continents = new List<Continent>();

		public void Create(int width, int height, float scale)
		{
			//float[,] noiseMap = new float[width, height];
			for (int z = 0; z < height; z++)
			{
				for (int x = 0; x < width; x++)
				{
					if (Mathf.PerlinNoise(x * scale, z * scale) < 0.5f)
						tiles.Add(new Tile(x, z, TileSprite.Land));
					else
						tiles.Add(new Tile(x, z));
					//Debug.Log(Mathf.PerlinNoise(x * scale, z * scale));

				//	Debug.Log(x * scale + ", " + z * scale + ": " + Mathf.PerlinNoise(width * scale, height * scale));
					//noiseMap[x, y] = Mathf.PerlinNoise(x * scale, y * scale);
					//Debug.Log(noiseMap[x, y]);

					//Debug.Log(Mathf.PerlinNoise(0.3f, 0.0f));
				}
			}
		}

		public List<Tile> GetTiles()
		{
			return tiles;
		}
	}

	//public class World
	//{
	//	string name;
	//	int size;
	//	int numberOfCharacters;

	//	public World(int size, string name)
	//	{
	//		int height = size;
	//		int width = size * 2;
	//		int[,] tiles = new int[height, width];
	//		//Add sprites
	//		for (int i = 0; i < height; i++)
	//		{
	//			//int tiles = 
	//		}	
	//	}
	//}
}