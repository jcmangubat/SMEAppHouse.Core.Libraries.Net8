using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class RegionNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.RegionName) || t == typeof(Rules.RegionName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.RegionName.Empty;
                case "Africa":
                    return Rules.RegionName.Africa;
                case "Americas":
                    return Rules.RegionName.Americas;
                case "Asia":
                    return Rules.RegionName.Asia;
                case "Europe":
                    return Rules.RegionName.Europe;
                case "Oceania":
                    return Rules.RegionName.Oceania;
            }
            throw new Exception("Cannot unmarshal type RegionName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.RegionName)untypedValue;
            switch (value)
            {
                case Rules.RegionName.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.RegionName.Africa:
                    serializer.Serialize(writer, "Africa");
                    return;
                case Rules.RegionName.Americas:
                    serializer.Serialize(writer, "Americas");
                    return;
                case Rules.RegionName.Asia:
                    serializer.Serialize(writer, "Asia");
                    return;
                case Rules.RegionName.Europe:
                    serializer.Serialize(writer, "Europe");
                    return;
                case Rules.RegionName.Oceania:
                    serializer.Serialize(writer, "Oceania");
                    return;
            }
            throw new Exception("Cannot marshal type RegionName");
        }

        public static readonly RegionNameConverter Singleton = new RegionNameConverter();
    }
}