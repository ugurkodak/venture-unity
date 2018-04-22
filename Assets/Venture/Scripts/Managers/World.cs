using System.Collections.Generic;
using UnityEngine;

namespace Venture.Managers
{
	public class World : MonoBehaviour
	{
		public static World Instance = null;
		public Prefabs.Tile Tile;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
		}

		void Start()
		{
			Data.World world = new Data.World().Create();
			foreach (Data.Tile tile in world.Tiles)
			{
				Debug.Log("Tile Instantiated");
				Prefabs.Tile newTile = Instantiate(Tile);
				newTile.Render(tile);
			}
		}
	}
}