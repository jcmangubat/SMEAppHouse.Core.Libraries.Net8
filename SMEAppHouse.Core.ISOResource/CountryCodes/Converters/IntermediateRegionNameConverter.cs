using Newtonsoft.Json;
using System;

namespace SMEAppHouse.Core.ISOResource.CountryCodes.Converters
{
    internal class IntermediateRegionNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rules.IntermediateRegionName) || t == typeof(Rules.IntermediateRegionName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Rules.IntermediateRegionName.Empty;
                case "Caribbean":
                    return Rules.IntermediateRegionName.Caribbean;
                case "Central America":
                    return Rules.IntermediateRegionName.CentralAmerica;
                case "Channel Islands":
                    return Rules.IntermediateRegionName.ChannelIslands;
                case "Eastern Africa":
                    return Rules.IntermediateRegionName.EasternAfrica;
                case "Middle Africa":
                    return Rules.IntermediateRegionName.MiddleAfrica;
                case "South America":
                    return Rules.IntermediateRegionName.SouthAmerica;
                case "Southern Africa":
                    return Rules.IntermediateRegionName.SouthernAfrica;
                case "Western Africa":
                    return Rules.IntermediateRegionName.WesternAfrica;
            }
            throw new Exception("Cannot unmarshal type IntermediateRegionName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rules.IntermediateRegionName)untypedValue;
            switch (value)
            {
                case Rules.IntermediateRegionName.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Rules.IntermediateRegionName.Caribbean:
                    serializer.Serialize(writer, "Caribbean");
                    return;
                case Rules.IntermediateRegionName.CentralAmerica:
                    serializer.Serialize(writer, "Central America");
                    return;
                case Rules.IntermediateRegionName.ChannelIslands:
                    serializer.Serialize(writer, "Channel Islands");
                    return;
                case Rules.IntermediateRegionName.EasternAfrica:
                    serializer.Serialize(writer, "Eastern Africa");
                    return;
                case Rules.IntermediateRegionName.MiddleAfrica:
                    serializer.Serialize(writer, "Middle Africa");
                    return;
                case Rules.IntermediateRegionName.SouthAmerica:
                    serializer.Serialize(writer, "South America");
                    return;
                case Rules.IntermediateRegionName.SouthernAfrica:
                    serializer.Serialize(writer, "Southern Africa");
                    return;
                case Rules.IntermediateRegionName.WesternAfrica:
                    serializer.Serialize(writer, "Western Africa");
                    return;
            }
            throw new Exception("Cannot marshal type IntermediateRegionName");
        }

        public static readonly IntermediateRegionNameConverter Singleton = new IntermediateRegionNameConverter();
    }
}