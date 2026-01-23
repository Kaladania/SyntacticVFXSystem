using UnityEngine;

namespace SnSECS
{
    //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ElementType
    {
        BASE,
        EXTRA,
        AMBIENCE,
        NONE
    }

    public enum Elements
    {
        FIRE,
        EARTH,
        WATER,
        LIGHTNING,
        NONE
    }

    public struct SNSData
    {
        public static int MAX_COMBO_LIMIT = 5; //states the maximum number of elements that can be added to a combination
        public static int NUM_ELEMENTS = 5; //states the maximum number of elements that can be added to a combination
    }
}