using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class GlobalNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.GlobalName) || t == typeof(Rules.GlobalName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.GlobalName.Empty;
                case "World":
                    return Rules.GlobalName.World;
            }
            throw new Exception("Cannot unmarshal type GlobalName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.GlobalName)untypedValue;
            switch (value)
            {
                case Rules.GlobalName.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.GlobalName.World:
                    serializer.Serialize(writer, "World");
                    return;
            }
            throw new Exception("Cannot marshal type GlobalName");
        }

        public static readonly GlobalNameConverter Singleton = new GlobalNameConverter();
    }
}