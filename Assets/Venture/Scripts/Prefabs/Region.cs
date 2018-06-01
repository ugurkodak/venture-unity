using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venture.Prefabs
{
	public class Region : MonoBehaviour
	{
		//private Data.Region data;
		public static Region Render(Data.Region data, Tile tilePrefab, Transform parent)
		{
			Region region = new GameObject().AddComponent<Region>(); //Fake prefab
			region.transform.position = data.GetPivot();
			region.transform.parent = parent;
			region.name = data.Name;
			
			Color color = new Color( //TODO: Handle region borders differently
					Random.Range(0.0f, 1.0f), Random.Range(0.8f, 1.0f), 1.0f, 1.0f);
			foreach (Data.Tile tile in data.Tiles)
				Tile.Render(tilePrefab, tile, region.transform, "region tile")
					.GetComponent<SpriteRenderer>().color = color;
			return region;
		}
	}
}
