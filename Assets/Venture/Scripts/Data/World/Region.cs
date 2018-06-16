using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture.Data
{
	public class Region
	{
		public RegionInfo Info;
		public DBList<RegionTile> Tiles;
		//TODO: public Resource[] Resources;

		public Region(string worldKey)
		{
			Info = new RegionInfo(worldKey);
		}

		public void Create(DBList<RegionTile> tiles)
		{
			Info.Create();
			Tiles = new DBList<RegionTile>(Info.Collection.Key + "/" + Info.Key);
			Tiles = tiles;
		}

		public async Task Update()
		{
			await Info.Update();
			await Tiles.Update();
		}

		public async Task Delete()
		{
			await Info.Delete();
			await Tiles.Delete();
		}

		public Vector3 GetPivot()
		{
			Vector3 pivot = new Vector3();
			foreach (RegionTile tile in Tiles)
				pivot += new Vector3(tile.X, 0, tile.Z);
			return pivot / Tiles.Count;
		}
	}
}