using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class Iso4217CurrencyMinorUnitUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Iso4217CurrencyMinorUnitUnion) || t == typeof(Iso4217CurrencyMinorUnitUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Iso4217CurrencyMinorUnitUnion { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    switch (stringValue)
                    {
                        case "":
                            return new Iso4217CurrencyMinorUnitUnion { Enum = Rules.Iso4217CurrencyMinorUnitEnum.Empty };
                        case "2,2":
                            return new Iso4217CurrencyMinorUnitUnion { Enum = Rules.Iso4217CurrencyMinorUnitEnum.The22 };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type Iso4217CurrencyMinorUnitUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Iso4217CurrencyMinorUnitUnion)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.Enum != null)
            {
                switch (value.Enum)
                {
                    case Rules.Iso4217CurrencyMinorUnitEnum.Empty:
                        serializer.Serialize(writer, "");
                        return;
                    case Rules.Iso4217CurrencyMinorUnitEnum.The22:
                        serializer.Serialize(writer, "2,2");
                        return;
                }
            }
            throw new Exception("Cannot marshal type Iso4217CurrencyMinorUnitUnion");
        }

        public static readonly Iso4217CurrencyMinorUnitUnionConverter Singleton = new Iso4217CurrencyMinorUnitUnionConverter();
    }
}