using System.Collections.Generic;

namespace Venture.Data
{
    // City data stored in memory
    public class City
    {
        public string Id;
        public CityMeta Meta;
        public CityRaw Raw;

        public City(string id, CityMeta meta)
        {
            Id = id;
            Meta = meta;
        }
    }

    // Modifiable values stored in city document
    public struct CityMeta
    {
        public string name;

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                ["name"] = name,
            };
        }
    }

    // Readonly values stored in city document 
    public struct CityRaw
    {
        public CityTile[] tiles;
    }

    public struct CityTile
    {
        public int x, y, lotId, blockId, areaId;
        public float value;
        public CityTileDirection direction;
        public CityTileTexture texture;
        public string characterId;
    }

    public enum CityTileTexture { ROAD, TRAMWAY, LAND }
    public enum CityTileDirection { NORTH, EAST, SOUTH, WEST }
}