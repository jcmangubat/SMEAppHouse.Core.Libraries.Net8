using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class GlobalCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.GlobalCode) || t == typeof(Rules.GlobalCode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.GlobalCode.Empty;
                case "True":
                    return Rules.GlobalCode.True;
            }
            throw new Exception("Cannot unmarshal type GlobalCode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.GlobalCode)untypedValue;
            switch (value)
            {
                case Rules.GlobalCode.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.GlobalCode.True:
                    serializer.Serialize(writer, "True");
                    return;
            }
            throw new Exception("Cannot marshal type GlobalCode");
        }

        public static readonly GlobalCodeConverter Singleton = new GlobalCodeConverter();
    }
}