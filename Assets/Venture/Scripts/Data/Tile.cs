using UnityEngine;

namespace Venture.Data
{
	[System.Serializable]
	public class Tile
	{
		public enum TileSprite { Water, Land, Empty };

		[SerializeField]
		private int x, z;
		public int X { get { return x; } private set { x = value; } }
		public int Z { get { return z; } private set { z = value; } }

		public Direction Direction { get; set; }
		public TileSprite Sprite { get; set; }

		public Tile(int x, int z, Direction direction, TileSprite sprite)
		{
			X = x;
			Z = z;
			Sprite = sprite;
			Direction = direction;
		}
	}
}