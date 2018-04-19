//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class City : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
		
//	}

//	//Generate tile positions spirrally expanding from 0,0,0
//	//floor((n^2)/4) makes a square or neat rectangle
//	void GenerateLot(int size)
//	{
//		int tileCount = (int)Mathf.Floor(size * size * 0.25f);
//		float x = 0;
//		float z = 0;
//		bool swap = false;
//		int direction = 1;
//		int steps = 1;
//		int remaininSteps = 1;
//		int turn = 2;

//		GameObject tile0 = Instantiate(Tile);
//		tile0.name = "0";
//		tile0.transform.position = new Vector3(0, 0, 0);

//		for (int i = 1; i < tileCount; i++)
//		{
//			GameObject tile = Instantiate(Tile);
//			tiles.Add(tile); //TODO: Remove

//			if (swap)
//				x = x + direction;
//			else
//				z = z + direction;
//			remaininSteps--;

//			if (remaininSteps == 0)
//			{
//				swap = !swap;
//				remaininSteps = steps;
//				turn--;
//			}

//			if (turn == 0)
//			{
//				direction = -direction;
//				steps++;
//				remaininSteps = steps;
//				turn = 2;
//			}

//			tile.name = i.ToString();
//			tile.transform.position = new Vector3(x, 0, z);
//		}
//	}
//}
