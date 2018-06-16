using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;

namespace Venture
{
	public class World : MonoBehaviour
	{
		public Sprite WaterTile, LandTile;
		public Data.World Data;
		public static World Instance;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			Data = new Data.World();
		}

		async void Start()
		{
			Data.Create();
			await Data.Update();
			Render();
			
			//await AddNew();
			//AddNew();
			//SetupSession(Character.Instance.Data.WorldId);
			//SetupSession("-LF-Msi7xLrg4xUHFuG3");
		}

		//public async Task SetupSession(string key)
		//{
		//	await Data.Load(key);
		//}

		//public async Task AddNew()
		//{
		//	Data.Create();
		//	await Data.Add();
		//}

		public void Render()
		{
			GameObject waters = new GameObject("Waters");
			waters.transform.parent = transform;
			foreach (Data.OceanTile tileData in Data.Waters)
			{
				GameObject tile = new GameObject();
				tile.AddComponent<SpriteRenderer>().sprite = WaterTile;
				tile.transform.position = new Vector3(tileData.X, 0, tileData.Z);
				tile.transform.eulerAngles = new Vector3(90, 0, 0);
				tile.transform.parent = waters.transform;
				tile.name = "tile_water";
			}

			GameObject regions = new GameObject("Regions");
			regions.transform.parent = transform;
			foreach (Data.Region regionData in Data.Regions)
			{
				GameObject region = new GameObject(regionData.Info.Name);
				region.transform.position = regionData.GetPivot();
				region.transform.parent = regions.transform;

				//TODO: Handle region borders/colors differently
				Color color = new Color(
					Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
				foreach (Data.RegionTile tileData in regionData.Tiles)
				{
					GameObject tile = new GameObject();
					tile.AddComponent<SpriteRenderer>().sprite = LandTile;
					tile.transform.position = new Vector3(tileData.X, 0, tileData.Z);
					tile.transform.eulerAngles = new Vector3(90, 0, 0);
					tile.transform.parent = region.transform;
					tile.name = "tile_region";
					tile.GetComponent<SpriteRenderer>().color = color;
				}
			}


			//Region.Render(regionData, Tile, regions.transform);
		}
		//public class Region : MonoBehaviour
		//{
		//	public static Region Render(Data.Region data, Tile tilePrefab, Transform parent)
		//	{
		//		Region region = new GameObject().AddComponent<Region>(); //Fake prefab
		//		region.transform.position = data.GetPivot();
		//		region.transform.parent = parent;
		//		//region.name = data.Name;

		//		Color color = new Color( //TODO: Handle region borders differently
		//				Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
		//		foreach (Data.RegionTile tile in data.Tiles)
		//			Tile.Render(tilePrefab, tile, region.transform, "land")
		//				.GetComponent<SpriteRenderer>().color = color;
		//		return region;
		//	}
		//}

	}
}