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
			Render(new Data.World().Create());
			data.Write();
		}

		public void Render(Data.World data)
		{
			this.data = data;
			name = data.Name;
			foreach (Data.Region region in data.Regions)
				Prefabs.Region.Render(region, Tile, transform);
			foreach (Data.Tile tile in data.Oceans)
				Prefabs.Tile.Render(Tile, tile, transform, "ocean tile");
		}
	}
}