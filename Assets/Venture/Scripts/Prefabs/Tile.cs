using UnityEngine;

namespace Venture.Prefabs
{
	public class Tile : MonoBehaviour
	{
		public Sprite Water, Land;
		public Data.Tile Data;
		private SpriteRenderer spriteRenderer;

		void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public static Tile Render(Tile prefab, Data.Tile data, Transform parent, string name)
		{
			Tile tile = Instantiate(prefab);
			tile.Data = data;
			//TODO: Set direction
			switch (data.Sprite)
			{
				case Venture.Data.Tile.TileSprite.Empty:
					tile.spriteRenderer.sprite = null;
					break;
				case Venture.Data.Tile.TileSprite.Water:
					tile.spriteRenderer.sprite = tile.Water;
					break;
				case Venture.Data.Tile.TileSprite.Land:
					tile.spriteRenderer.sprite = tile.Land;
					break;
			}
			tile.transform.position = new Vector3(data.X, 0, data.Z);
			tile.transform.parent = parent;
			tile.name = name;
			return tile;
		}
		public static Tile Render(Tile prefab, Data.Tile data, Transform parent)
		{
			return Render(prefab, data, parent, "tile");
		}
		public static Tile Render(Tile prefab, Data.Tile data)
		{
			return Render(prefab, data, null, "tile");
		}
	}
}
