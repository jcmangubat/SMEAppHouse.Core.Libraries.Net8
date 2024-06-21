using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class Iso4217CurrencyMinorUnitEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.Iso4217CurrencyMinorUnitEnum) || t == typeof(Rules.Iso4217CurrencyMinorUnitEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.Iso4217CurrencyMinorUnitEnum.Empty;
                case "2,2":
                    return Rules.Iso4217CurrencyMinorUnitEnum.The22;
            }
            throw new Exception("Cannot unmarshal type Iso4217CurrencyMinorUnitEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.Iso4217CurrencyMinorUnitEnum)untypedValue;
            switch (value)
            {
                case Rules.Iso4217CurrencyMinorUnitEnum.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.Iso4217CurrencyMinorUnitEnum.The22:
                    serializer.Serialize(writer, "2,2");
                    return;
            }
            throw new Exception("Cannot marshal type Iso4217CurrencyMinorUnitEnum");
        }

        public static readonly Iso4217CurrencyMinorUnitEnumConverter Singleton = new Iso4217CurrencyMinorUnitEnumConverter();
    }
}