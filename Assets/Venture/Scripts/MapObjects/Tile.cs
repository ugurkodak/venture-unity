using UnityEngine;
using Venture.Data;

namespace Venture.Prefabs
{
	public class Tile : MonoBehaviour
	{
		private Data.Tile data;
		private SpriteRenderer spriteRenderer;
		public Sprite Water, Land;

		void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Render(Data.Tile data)
		{
			this.data = data;
			transform.position = new Vector3(data.X, 0, data.Z);
			//TODO: Set direction
			switch (data.Sprite)
			{
				case Data.Tile.TileSprite.Empty:
					spriteRenderer.sprite = null;
					break;
				case Data.Tile.TileSprite.Water:
					spriteRenderer.sprite = Water;
					break;
				case Data.Tile.TileSprite.Land:
					spriteRenderer.sprite = Land;
					break;
			}
		}
	}
}
