using UnityEngine;

namespace Venture
{
	public class World : MonoBehaviour
	{
		public Tile Tile;
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

		void Start()
		{
			//Data.Create();
			Render(Character.Instance.Data.WorldId);
			//Render("-LElrAqQ0FK4iDHCNkN-");
		}

		public async void Render(string key)
		{
			await Data.Read(key);
			name = Data.Info.Name;

			GameObject waters = new GameObject("WorldWaters");
			waters.transform.parent = transform;
			foreach (Data.Tile tileData in Data.Waters.List)
				Tile.Render(Tile, tileData, waters.transform, "water");

			GameObject regions = new GameObject("WorldRegions");
			regions.transform.parent = transform;
			foreach (Data.Region regionData in Data.Regions.List)
				Region.Render(regionData, Tile, regions.transform);
		}

		//public void Render()
		//{
		//	name = Data.Info.Name;
		//	foreach (Data.Region region in Data.Regions)
		//		Prefabs.Region.Render(region, Tile, transform);
		//	foreach (Data.Tile tile in Data.Waters)
		//		Prefabs.Tile.Render(Tile, tile, transform, "ocean tile");
		//}
	}
}