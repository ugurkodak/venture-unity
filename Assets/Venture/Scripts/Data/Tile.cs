using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

namespace Venture.Data
{
	public class Tile
	{
		private int x, z;
		private Direction direction;
		private TileSprite sprite;

		public Tile(int x, int z,
			TileSprite sprite = TileSprite.Water,
			Direction direction = Direction.North)
		{
			this.x = x;
			this.z = z;
			this.sprite = sprite;
			this.direction = direction;
		}

		//public void Write(DatabaseReference reference)
		//{
		//	reference.Child(reference.Push().Key)
		//		.SetRawJsonValueAsync(JsonUtility.ToJson(this))
		//		.ContinueWith(task =>
		//		{
		//			if (task.IsCompleted)
		//				Debug.Log("Tile added.");
		//			else
		//				Debug.Log("Problem adding tile");
		//		});
		//}

		public Vector3 GetPosition()
		{
			return new Vector3(x, 0, z);
		}

		public TileSprite GetSprite()
		{
			return sprite;
		}
	}
}