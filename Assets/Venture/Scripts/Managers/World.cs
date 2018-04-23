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
		}

		public void Render(Data.World data)
		{
			this.data = data;
			//Render region tiles and organise container gameobjects
			foreach (Data.Region region in data.Regions)
			{
				GameObject newRegion = new GameObject();
				newRegion.transform.position = region.GetPivot();
				newRegion.transform.parent = transform;
				newRegion.name = "Region";
				Color color = new Color(
						Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
				foreach (Data.Tile tile in region.Tiles)
				{
					Prefabs.Tile newTile = Instantiate(Tile);
					newTile.transform.parent = newRegion.transform;
					newTile.Render(tile);
					newTile.GetComponent<SpriteRenderer>().color = color;
				}
			}
			//Render ocean tiles
			foreach (Data.Tile tile in data.Oceans)
			{
				Prefabs.Tile newTile = Instantiate(Tile);
				newTile.transform.parent = transform;
				newTile.Render(tile);
				newTile.name = "Ocean";
			}
		}
	}
}