using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class ContinentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.Continent) || t == typeof(Rules.Continent?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.Continent.Empty;
                case "AF":
                    return Rules.Continent.Af;
                case "AN":
                    return Rules.Continent.An;
                case "AS":
                    return Rules.Continent.As;
                case "EU":
                    return Rules.Continent.Eu;
                case "NA":
                    return Rules.Continent.Na;
                case "OC":
                    return Rules.Continent.Oc;
                case "SA":
                    return Rules.Continent.Sa;
            }
            throw new Exception("Cannot unmarshal type Continent");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.Continent)untypedValue;
            switch (value)
            {
                case Rules.Continent.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.Continent.Af:
                    serializer.Serialize(writer, "AF");
                    return;
                case Rules.Continent.An:
                    serializer.Serialize(writer, "AN");
                    return;
                case Rules.Continent.As:
                    serializer.Serialize(writer, "AS");
                    return;
                case Rules.Continent.Eu:
                    serializer.Serialize(writer, "EU");
                    return;
                case Rules.Continent.Na:
                    serializer.Serialize(writer, "NA");
                    return;
                case Rules.Continent.Oc:
                    serializer.Serialize(writer, "OC");
                    return;
                case Rules.Continent.Sa:
                    serializer.Serialize(writer, "SA");
                    return;
            }
            throw new Exception("Cannot marshal type Continent");
        }

        public static readonly ContinentConverter Singleton = new ContinentConverter();
    }
}