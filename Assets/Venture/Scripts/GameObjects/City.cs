using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using Venture.Data;
using System.Collections.Generic;
using System;

namespace Venture
{
	public class City : MonoBehaviour
	{
		class CityInfo : DBModel
		{
			//TODO: Remove temporary database placeholders.
			private List<string> worldNames = new List<string> { "CityName1", "CityName2", "CityName2", "CityName3" };

			public string Name;
			public string DateCreated;

			public CityInfo() : base(null) { }

			public CityInfo Create()
			{
				DateCreated = DateTime.Now.ToString(Access.DATE_TIME_FORMAT);
				Name = worldNames[UnityEngine.Random.Range(0, worldNames.Count)];
				return this;
			}
		}
		class LotTile : DBModel
		{
			public int X;
			public int Z;
			public LotTile(string worldKey, string cityKey, string lotKey) : base(worldKey, cityKey, lotKey) { }

			public void Create(int x, int z)
			{
				X = x;
				Z = z;
			}
		}
		class StreetTile : DBModel
		{
			public int X;
			public int Z;
			Direction Direction;

			public StreetTile(string worldKey, string cityKey) : base(worldKey, cityKey) { }

			public void Create(int x, int z, Direction direction)
			{
				X = x;
				Z = z;
				Direction = direction;
			}
		}
		class LotInfo : DBModel
		{
			//Bank is government for now.
			public enum BuildingType { Empty, Bank, Store };

			public string CharacterId;
			public bool IsGovernment;
			public BuildingType Building;

			public LotInfo(string worldKey, string cityKey) : base(worldKey, cityKey) { }

			public void Create(string characterId, bool isGovernment, BuildingType building)
			{
				CharacterId = characterId;
				IsGovernment = isGovernment;
				Building = building;
			}
		}

		CityInfo Info;
		DBList<LotInfo> LotInfos;
		List<DBList<LotTile>> Lots;
		DBList<StreetTile> Streets;
		DBList<StreetTile> Tramways;

		public static City Instance;
		public Sprite StreetTileSprite, TramwayTileSprite, TramwayIntersectionTileSprite, LandTileSprite;

		const int Width = 30, Height = 30;
		const int LotWidth = 4, LotHeight = 4;


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
			Create();
			Render();
			//await Load(Character.Instance.Data.WorldId);
		}

		//TODO: Add world Key
		//TODO: Make city creation procedural
		public void Create()
		{
			Info = new CityInfo();
			Info.Create();
			Tramways = new DBList<StreetTile>(Info.Key);
			Streets = new DBList<StreetTile>(Info.Key);
			//string[,] Tiles = new string[Height, Width];
			//List<string> lots;
			for (int z = 0; z < Height; z++)
				for (int x = 0; x < Width; x++)
				{
					if (x == 14 || x == 15 || z == 14 || z == 15)
					{
						StreetTile tile = new StreetTile("WorldKey", Info.Key);
						tile.Create(x, z, Direction.North);
						Tramways.Add(tile);
					}
					else if (((x < 14 && (x % 5 == 4)) || (z < 14 && (z % 5 == 4))) ||
						((x > 15 && (x % 5 == 0)) || (z > 15 && (z % 5 == 0))))
					{
						StreetTile tile = new StreetTile("WorldKey", Info.Key);
						tile.Create(x, z, Direction.North);
						Streets.Add(tile);
					}
				}
		}

		public void Render()
		{
			foreach (StreetTile tramwayTile in Tramways)
			{
				GameObject tile = new GameObject();
				tile.AddComponent<SpriteRenderer>().sprite = TramwayTileSprite;
				tile.transform.position = new Vector3(tramwayTile.X, 0, tramwayTile.Z);
				tile.transform.eulerAngles = new Vector3(90, 0, 0);
				tile.transform.parent = transform;
				tile.name = "tile_tramway";
			}

			foreach (StreetTile streetTile in Streets)
			{
				GameObject tile = new GameObject();
				tile.AddComponent<SpriteRenderer>().sprite = StreetTileSprite;
				tile.transform.position = new Vector3(streetTile.X, 0, streetTile.Z);
				tile.transform.eulerAngles = new Vector3(90, 0, 0);
				tile.transform.parent = transform;
				tile.name = "tile_street";
			}

			//GameObject waters = new GameObject("Waters");
			//waters.transform.parent = transform;
			//foreach (Data.FarmlandTile tileData in Data.OceanTiles)
			//{
			//	GameObject tile = new GameObject();
			//	tile.AddComponent<SpriteRenderer>().sprite = WaterTile;
			//	tile.transform.position = new Vector3(tileData.X, 0, tileData.Z);
			//	tile.transform.eulerAngles = new Vector3(90, 0, 0);
			//	tile.transform.parent = waters.transform;
			//	tile.name = "tile_water";
			//}

			//GameObject regions = new GameObject("Regions");
			//regions.transform.parent = transform;
			//foreach (Data.DBList<Data.BlockTile> regionTiles in Data.RegionTiles)
			//{
			//	Data.BlockInfo regionInfo = Data.RegionInfos.GetItem(regionTiles.Reference.Key);
			//	GameObject region = new GameObject(regionInfo.Name);

			//	//Center region gameobject to center of its tiles
			//	Vector3 pivot = new Vector3();
			//	foreach (Data.BlockTile tile in regionTiles)
			//		pivot += new Vector3(tile.X, 0, tile.Z);
			//	region.transform.position = pivot / regionTiles.Count;
			//	region.transform.parent = regions.transform;

			//	//TODO: Handle region borders/colors differently
			//	Color color = new Color(
			//			Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
			//	foreach (Data.BlockTile tileData in regionTiles)
			//	{
			//		GameObject tile = new GameObject();
			//		tile.AddComponent<SpriteRenderer>().sprite = LandTile;
			//		tile.transform.position = new Vector3(tileData.X, 0, tileData.Z);
			//		tile.transform.eulerAngles = new Vector3(90, 0, 0);
			//		tile.transform.parent = region.transform;
			//		tile.name = "tile_region";
			//		tile.GetComponent<SpriteRenderer>().color = color;
			//	}
			//}
		}
	}
}