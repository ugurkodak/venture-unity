using UnityEngine;
using Venture.Data;

public class GameTile : MonoBehaviour {
	SpriteRenderer spriteRenderer;

	TileSprite tileType = TileSprite.Water;
	public enum Direction : int {North, East, South, West};
	public Sprite Land;
	public Sprite Water;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetTileType(TileSprite sprite)
	{
		switch (sprite)
		{
			case TileSprite.Empty:
				spriteRenderer.sprite = null;
				tileType = TileSprite.Empty;
				break;
			case TileSprite.Land:
				spriteRenderer.sprite = Land;
				tileType = TileSprite.Land;
				break;
			case TileSprite.Water:
				spriteRenderer.sprite = Water;
				tileType = TileSprite.Water;
				break;
			default:
				spriteRenderer.sprite = Water;
				tileType = TileSprite.Water;
				break;
		}
	}

	public TileSprite GetTileType()
	{
		return tileType;
	}
}
