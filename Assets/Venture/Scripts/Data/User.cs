using System.Collections.Generic;

namespace Venture.Data
{
    // User data stored in memory
    public class User
    {
        public readonly string Id;
        public UserMeta Meta;

        public User(string id, UserMeta meta)
        {
            Id = id;
            Meta = meta;
        }
    }

    // Modifiable values stored in a user document
    public struct UserMeta
    {
        public string characterId;

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                ["characterId"] = characterId
            };
        }
    }
}