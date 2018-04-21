namespace Venture.Data
{
	public enum Direction { North, East, South, West };
	public enum TileSprite { Water, Land, Empty };

	public struct MapAreaInfo
	{
		public int x, z, size;
		public string name;

		public MapAreaInfo(int x, int z, int size, string name)
		{
			this.x = x;
			this.z = z;
			this.size = size;
			this.name = name;
		}
	}

	public struct Tile
	{
		public int x;
		public int z;
		public Direction direction;
		public TileSprite sprite;

		public Tile(int x, int z, Direction direction, TileSprite sprite)
		{
			this.x = x;
			this.z = z;
			this.sprite = sprite;
			this.direction = direction;
		}

		public void Set(int x, int z, Direction direction, TileSprite sprite)
		{
			this.x = x;
			this.z = z;
			this.sprite = sprite;
			this.direction = direction;
		}
	}
}