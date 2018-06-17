namespace Venture.Data
{
	public class RegionTile : DBModel
	{
		public int X;
		public int Z;

		public RegionTile(string worldKey, string regionKey) : base(worldKey, regionKey) { }

		public void Create(int x, int z)
		{
			X = x;
			Z = z;
		}
	}
}