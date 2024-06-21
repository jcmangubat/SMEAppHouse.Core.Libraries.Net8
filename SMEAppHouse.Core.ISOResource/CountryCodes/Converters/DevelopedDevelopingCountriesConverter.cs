using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class DevelopedDevelopingCountriesConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.DevelopedDevelopingCountries) || t == typeof(Rules.DevelopedDevelopingCountries?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.DevelopedDevelopingCountries.Empty;
                case "Developed":
                    return Rules.DevelopedDevelopingCountries.Developed;
                case "Developing":
                    return Rules.DevelopedDevelopingCountries.Developing;
            }
            throw new Exception("Cannot unmarshal type DevelopedDevelopingCountries");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.DevelopedDevelopingCountries)untypedValue;
            switch (value)
            {
                case Rules.DevelopedDevelopingCountries.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.DevelopedDevelopingCountries.Developed:
                    serializer.Serialize(writer, "Developed");
                    return;
                case Rules.DevelopedDevelopingCountries.Developing:
                    serializer.Serialize(writer, "Developing");
                    return;
            }
            throw new Exception("Cannot marshal type DevelopedDevelopingCountries");
        }

        public static readonly DevelopedDevelopingCountriesConverter Singleton = new DevelopedDevelopingCountriesConverter();
    }
}