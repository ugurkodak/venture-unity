using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public static World Instance = null;
	public GameObject Tile;

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
		CreateWorldTiles();
	}

	//Size = floor((n^2)/4)
	void CreateWorldTiles()
	{
		Venture.Instance.Database.Child("tiles").Child(Character.Instance.WorldId).GetValueAsync()
		.ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					foreach (var position in task.Result.Children)
					{
						GameObject tile = Instantiate(Tile);
						tile.name = position.Key;
						tile.transform.position = new Vector3(
						float.Parse((position.Value as IDictionary<string, object>)["x"].ToString()),
						float.Parse((position.Value as IDictionary<string, object>)["y"].ToString()),
						float.Parse((position.Value as IDictionary<string, object>)["z"].ToString()));
					}
				}
			}
		});
	}
}
