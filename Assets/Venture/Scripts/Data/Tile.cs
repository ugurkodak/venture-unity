using UnityEngine;

namespace Venture.Data
{
	public class Tile
	{
		public enum TileSprite { Water, Land, Empty };

		public int X { get; private set; }
		public int Z { get; private set; }
		public Direction Direction { get; private set; }
		public TileSprite Sprite { get; private set; }

		public Tile(int x, int z, Direction direction, TileSprite sprite)
		{
			X = x;
			Z = z;
			Sprite = sprite;
			Direction = direction;
		}
	}
}