namespace Venture.Data
{
	public enum Direction { North, East, South, West };

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
}