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
			//Data.Create();
			//await Data.Update();
			await Data.Load("-LFAJZIOapn1RAmcojp0");
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
			foreach (Data.OceanTile tileData in Data.OceanTiles)
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
			foreach (Data.DBList<Data.RegionTile> regionTiles in Data.RegionTiles)
			{
				Data.RegionInfo regionInfo = Data.RegionInfos.GetItem(regionTiles.Reference.Key);
				GameObject region = new GameObject(regionInfo.Name);

				//Center region gameobject to center of its tiles
				Vector3 pivot = new Vector3();
				foreach (Data.RegionTile tile in regionTiles)
					pivot += new Vector3(tile.X, 0, tile.Z);
				region.transform.position = pivot / regionTiles.Count;
				region.transform.parent = regions.transform;

				//TODO: Handle region borders/colors differently
				Color color = new Color(
						Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
				foreach (Data.RegionTile tileData in regionTiles)
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
		}
	}
}