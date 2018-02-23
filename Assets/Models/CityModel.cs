using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityModel
{
    TileModel[,] tiles;
	public int Width { get; private set; }
    public int Height { get; private set; }
	public int Size { get; private set; }
	

    public CityModel(int Width = 10, int Height = 10)
    {
        this.Width = Width;
        this.Height = Height;
		Size = Width * Height;

        tiles = new TileModel[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                tiles[x, z] = new TileModel(this, new Vector3(x, 0, z));
            }
        }

        Debug.Log("Created " + Width * Height + " tiles.");
    }
}
