using System.Collections.Generic;

namespace Venture.Data
{
    // Character data stored in memory
    public class Character
    {
        public readonly string Id;
        public CharacterMeta Meta;
        public CharacterRaw Raw;

        public Character(string id, CharacterMeta meta)
        {
            Id = id;
            Meta = meta;
        }
    }

    // Modifiable values stored in a character document
    public struct CharacterMeta
    {
        public string firstName, lastName;
        public CharacterPrefix prefix;

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                ["firstName"] = firstName,
                ["lastName"] = lastName,
                ["prefix"] = (long)prefix
            };
        }
    }

    // Readonly values stored in a character document 
    public struct CharacterRaw
    {
        public string dateFirst, dateLast;
        public double balance;
    }

    public enum CharacterPrefix { MR, MS, MX }
}