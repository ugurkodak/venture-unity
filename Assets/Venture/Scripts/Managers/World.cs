using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Venture.Managers
{
	/*TODO: World sizes fixed until more functionality like resources, 
	city areas and creating new cities.
	Needs a complete rewrite*/
	public class World : MonoBehaviour
	{
		public static World Instance = null;
		public MapTile Tile;
		List<MapTile> tiles = new List<MapTile>();

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
		}

		void Start()
		{
			//Data.Region region = new Data.Region();
			//region.Create("Region Name", 0, 0, 6);
			//Debug.Log(region.GetWidth());
			//foreach (Data.Tile tile in region.GetTiles())
			//{
			//	MapTile newTile = Instantiate(Tile);
			//	newTile.transform.position = tile.GetPosition();
			//}

			Data.World world = new Data.World();
			world.Create(16, 8, 0.1f);
			foreach (Data.Tile tile in world.GetTiles())
			{
				MapTile newTile = Instantiate(Tile);
				newTile.SetTileType(tile.GetSprite());
				newTile.transform.position = tile.GetPosition();
			}
			//GenerateAndRender(3);
			//RenderFromData();
		}

		////Height and width has to be odd.
		//void GenerateAndRender(int size)
		//{
		//	//Instantiate tiles
		//	int width = 9 * size;
		//	int height = 7 * size;
		//	int firstX = -(int)Mathf.Ceil(width / 2);
		//	int firstZ = -(int)Mathf.Ceil(height / 2);
		//	for (int z = firstZ; z <= -firstZ; z++)
		//	{
		//		for (int x = firstX; x <= -firstX; x++)
		//		{
		//			MapTile tile = Instantiate(Tile);
		//			tile.transform.position = new Vector3(x, 0, z);
		//			tiles.Add(tile);
		//		}
		//	}

		//	//Pick land centers
		//	Vector3[] landCenters = new Vector3[size];
		//	int numberOfLandCentersToBePicked = size;
		//	while (numberOfLandCentersToBePicked > 0)
		//	{
		//		bool exists = false;
		//		Vector3 randomPosition = tiles[Random.Range(0, tiles.Count)].transform.position;
		//		foreach (Vector3 center in landCenters)
		//			if (center == randomPosition)
		//				exists = true;
		//		if (!exists)
		//		{
		//			numberOfLandCentersToBePicked--;
		//			landCenters[numberOfLandCentersToBePicked] = randomPosition;
		//		}
		//	}

		//	//Calculate distances
		//	List<float> distances = new List<float>();
		//	foreach (Vector3 center in landCenters)
		//	{
		//		foreach (MapTile tile in tiles)
		//		{
		//			float distance = Vector3.Distance(center, tile.transform.position);
		//			distances.Add(distance);
		//		}
		//	}

		//	//Set land tiles
		//	foreach (Vector3 center in landCenters)
		//	{
		//		foreach (MapTile tile in tiles)
		//		{
		//			float distance = Vector3.Distance(center, tile.transform.position);
		//			float probability = 100 - (Mathf.Pow(distance / distances.ToArray().Max(), 1) * 100);
		//			print(probability);
		//			bool[] distribution = new bool[100];
		//			for (int i = 0; i < distribution.Length; i++)
		//				distribution[i] = false;
		//			for (int i = 0; i < probability; i++)
		//				distribution[i] = true;
		//			bool isSet = distribution[Random.Range(0, distribution.Length)];
		//			if (isSet && tile.GetTileType() != MapTile.TileSprite.Empty)
		//				tile.SetTileType(MapTile.TileSprite.Land);

		//			if (tile.transform.position == center)
		//			{
		//				tile.SetTileType(MapTile.TileSprite.Empty);
		//				tile.name = "Center";
		//			}
		//		}
		//	}
		//}

		//void RenderFromData()
		//{
		//	Game.Instance.DatabaseRootReference.Child("tiles").Child(Character.Instance.WorldId).GetValueAsync()
		//	.ContinueWith(task =>
		//	{
		//		if (task.IsCompleted && task.Result.Exists)
		//		{
		//			foreach (var position in task.Result.Children)
		//			{
		//				MapTile tile = Instantiate(Tile);
		//				tile.name = position.Key;
		//				tile.transform.position = new Vector3(
		//				float.Parse((position.Value as IDictionary<string, object>)["x"].ToString()),
		//				float.Parse((position.Value as IDictionary<string, object>)["y"].ToString()),
		//				float.Parse((position.Value as IDictionary<string, object>)["z"].ToString()));
		//			}
		//		}
		//	});
		//}
	}
}