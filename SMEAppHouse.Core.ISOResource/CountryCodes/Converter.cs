using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SMEAppHouse.Core.ISOResource.CountryCodes.Converters;
using System.Globalization;

namespace SMEAppHouse.Core.ISOResource.CountryCodes
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ContinentConverter.Singleton,
                DevelopedDevelopingCountriesConverter.Singleton,
                DialConverter.Singleton,
                GlobalCodeConverter.Singleton,
                GlobalNameConverter.Singleton,
                Iso4217CurrencyMinorUnitUnionConverter.Singleton,
                Iso4217CurrencyMinorUnitEnumConverter.Singleton,
                IntermediateRegionNameConverter.Singleton,
                LandLockedDevelopingCountriesLldcConverter.Singleton,
                RegionNameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}