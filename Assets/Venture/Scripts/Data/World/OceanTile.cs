namespace Venture.Data
{
	public class OceanTile : DBModel
	{
		public int X;
		public int Z;

		public OceanTile(string worldKey) : base(worldKey) { }

		public void Create(int x, int z)
		{
			X = x;
			Z = z;
		}
	}
}