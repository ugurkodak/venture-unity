using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Data
{
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
					//if (Mathf.PerlinNoise(x * scale, z * scale) < 0.5f)
						//tiles.Add(new Tile(x, z, TileSprite.Land));
					//else
					//	tiles.Add(new Tile(x, z));
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
