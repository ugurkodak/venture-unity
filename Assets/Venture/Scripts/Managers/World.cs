using System.Collections.Generic;
using UnityEngine;

namespace Venture.Managers
{
	public class World : MonoBehaviour
	{
		public Prefabs.Tile Tile;
		private Data.World data;

		public static World Instance = null;
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
			//TODO: Read world data acording to character worldID
			Render(new Data.World().Create());
			//data.Write();
			//data = new Data.World();
			//data.Read("-LBNqPJ7lz0uqUEx53_f");
		}

		public void Render(Data.World data)
		{
			this.data = data;
			name = data.Meta.Name;
			foreach (Data.Region region in data.Regions)
				Prefabs.Region.Render(region, Tile, transform);
			foreach (Data.Tile tile in data.Oceans)
				Prefabs.Tile.Render(Tile, tile, transform, "ocean tile");
		}
	}
}