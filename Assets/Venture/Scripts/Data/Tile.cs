namespace Venture.Data
{
	public struct Tile
	{
		public enum TileSprite { Empty, Water, Land };

		public int X;
		public int Z;
		public Direction Direction;
		public TileSprite Sprite;

		public Tile(int x, int z, Direction direction, TileSprite sprite)
		{
			X = x;
			Z = z;
			Direction = direction;
			Sprite = sprite;
		}
	}
}