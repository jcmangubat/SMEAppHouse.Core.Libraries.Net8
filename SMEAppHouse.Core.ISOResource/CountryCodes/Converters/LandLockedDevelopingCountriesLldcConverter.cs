using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class LandLockedDevelopingCountriesLldcConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.LandLockedDevelopingCountriesLldc) || t == typeof(Rules.LandLockedDevelopingCountriesLldc?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.LandLockedDevelopingCountriesLldc.Empty;
                case "x":
                    return Rules.LandLockedDevelopingCountriesLldc.X;
            }
            throw new Exception("Cannot unmarshal type LandLockedDevelopingCountriesLldc");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.LandLockedDevelopingCountriesLldc)untypedValue;
            switch (value)
            {
                case Rules.LandLockedDevelopingCountriesLldc.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.LandLockedDevelopingCountriesLldc.X:
                    serializer.Serialize(writer, "x");
                    return;
            }
            throw new Exception("Cannot marshal type LandLockedDevelopingCountriesLldc");
        }

        public static readonly LandLockedDevelopingCountriesLldcConverter Singleton = new LandLockedDevelopingCountriesLldcConverter();
    }
}