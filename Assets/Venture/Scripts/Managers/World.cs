using System.Collections.Generic;
using UnityEngine;

namespace Venture.Managers
{
	public class World : MonoBehaviour
	{
		public Prefabs.Tile Tile;
		public Data.World Data;

		public static World Instance = null;
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
			//TODO: Read world data acording to character worldID
			//Render(new Data.World().Create());
			//Debug.Log(Character.Instance.Data.WorldId);
			Data.Read(Character.Instance.Data.WorldId);
			//data.Write();
			//data = new Data.World();
			//data.Read("-LBNqPJ7lz0uqUEx53_f");
		}

		public void Render()
		{
			name = Data.Meta.Name;
			foreach (Data.Region region in Data.Regions)
				Prefabs.Region.Render(region, Tile, transform);
			foreach (Data.Tile tile in Data.Oceans)
				Prefabs.Tile.Render(Tile, tile, transform, "ocean tile");
		}
	}
}