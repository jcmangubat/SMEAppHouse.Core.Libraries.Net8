﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using SMEAppHouse.Core.ISOResource;
//
//    var country = Country.FromJson(jsonString);

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Converters;
using SMEAppHouse.Core.ISOResource.CountryCodes.Converters;
using System.Reflection;
using System.IO;
using System.Linq;

namespace SMEAppHouse.Core.ISOResource.CountryCodes
{
    public partial class Country
    {
        #region properties

        [JsonProperty("official_name_ar")]
        public string OfficialNameAr { get; set; }

        [JsonProperty("official_name_cn")]
        public string OfficialNameCn { get; set; }

        [JsonProperty("official_name_en")]
        public string OfficialNameEn { get; set; }

        [JsonProperty("official_name_es")]
        public string OfficialNameEs { get; set; }

        [JsonProperty("official_name_fr")]
        public string OfficialNameFr { get; set; }

        [JsonProperty("official_name_ru")]
        public string OfficialNameRu { get; set; }

        [JsonProperty("ISO3166-1-Alpha-2")]
        public string Iso31661Alpha2 { get; set; }

        [JsonProperty("ISO3166-1-Alpha-3")]
        public string Iso31661Alpha3 { get; set; }

        [JsonProperty("ISO3166-1-numeric")]
        public Dial Iso31661Numeric { get; set; }

        [JsonProperty("ISO4217-currency_alphabetic_code")]
        public string Iso4217CurrencyAlphabeticCode { get; set; }

        [JsonProperty("ISO4217-currency_country_name")]
        public string Iso4217CurrencyCountryName { get; set; }

        [JsonProperty("ISO4217-currency_minor_unit")]
        public Iso4217CurrencyMinorUnitUnion Iso4217CurrencyMinorUnit { get; set; }

        [JsonProperty("ISO4217-currency_name")]
        public string Iso4217CurrencyName { get; set; }

        [JsonProperty("ISO4217-currency_numeric_code")]
        public Dial Iso4217CurrencyNumericCode { get; set; }

        [JsonProperty("M49")]
        public Dial M49 { get; set; }

        [JsonProperty("UNTERM Arabic Formal")]
        public string UntermArabicFormal { get; set; }

        [JsonProperty("UNTERM Arabic Short")]
        public string UntermArabicShort { get; set; }

        [JsonProperty("UNTERM Chinese Formal")]
        public string UntermChineseFormal { get; set; }

        [JsonProperty("UNTERM Chinese Short")]
        public string UntermChineseShort { get; set; }

        [JsonProperty("UNTERM English Formal")]
        public string UntermEnglishFormal { get; set; }

        [JsonProperty("UNTERM English Short")]
        public string UntermEnglishShort { get; set; }

        [JsonProperty("UNTERM French Formal")]
        public string UntermFrenchFormal { get; set; }

        [JsonProperty("UNTERM French Short")]
        public string UntermFrenchShort { get; set; }

        [JsonProperty("UNTERM Russian Formal")]
        public string UntermRussianFormal { get; set; }

        [JsonProperty("UNTERM Russian Short")]
        public string UntermRussianShort { get; set; }

        [JsonProperty("UNTERM Spanish Formal")]
        public string UntermSpanishFormal { get; set; }

        [JsonProperty("UNTERM Spanish Short")]
        public string UntermSpanishShort { get; set; }

        [JsonProperty("CLDR display name")]
        public string CldrDisplayName { get; set; }

        [JsonProperty("Capital")]
        public string Capital { get; set; }

        [JsonProperty("Continent")]
        public Rules.Continent Continent { get; set; }

        [JsonProperty("DS")]
        public string Ds { get; set; }

        [JsonProperty("Developed / Developing Countries")]
        public Rules.DevelopedDevelopingCountries DevelopedDevelopingCountries { get; set; }

        [JsonProperty("Dial")]
        public Dial Dial { get; set; }

        [JsonProperty("EDGAR")]
        public string Edgar { get; set; }

        [JsonProperty("FIFA")]
        public string Fifa { get; set; }

        [JsonProperty("FIPS")]
        public string Fips { get; set; }

        [JsonProperty("GAUL")]
        public Dial Gaul { get; set; }

        [JsonProperty("Geoname ID")]
        public Dial GeonameId { get; set; }

        [JsonProperty("Global Code")]
        public Rules.GlobalCode GlobalCode { get; set; }

        [JsonProperty("Global Name")]
        public Rules.GlobalName GlobalName { get; set; }

        [JsonProperty("IOC")]
        public string Ioc { get; set; }

        [JsonProperty("ITU")]
        public string Itu { get; set; }

        [JsonProperty("Intermediate Region Code")]
        public Dial IntermediateRegionCode { get; set; }

        [JsonProperty("Intermediate Region Name")]
        public Rules.IntermediateRegionName IntermediateRegionName { get; set; }

        [JsonProperty("Land Locked Developing Countries (LLDC)")]
        public Rules.LandLockedDevelopingCountriesLldc LandLockedDevelopingCountriesLldc { get; set; }

        [JsonProperty("Languages")]
        public string Languages { get; set; }

        [JsonProperty("Least Developed Countries (LDC)")]
        public Rules.LandLockedDevelopingCountriesLldc LeastDevelopedCountriesLdc { get; set; }

        [JsonProperty("MARC")]
        public string Marc { get; set; }

        [JsonProperty("Region Code")]
        public Dial RegionCode { get; set; }

        [JsonProperty("Region Name")]
        public Rules.RegionName RegionName { get; set; }

        [JsonProperty("Small Island Developing States (SIDS)")]
        public Rules.LandLockedDevelopingCountriesLldc SmallIslandDevelopingStatesSids { get; set; }

        [JsonProperty("Sub-region Code")]
        public Dial SubRegionCode { get; set; }

        [JsonProperty("Sub-region Name")]
        public string SubRegionName { get; set; }

        [JsonProperty("TLD")]
        public string Tld { get; set; }

        [JsonProperty("WMO")]
        public string Wmo { get; set; }

        [JsonProperty("is_independent")]
        public string IsIndependent { get; set; }

        #endregion properties
    }

    public partial struct Dial
    {
        public long? Integer;
        public string String;

        public static implicit operator Dial(long Integer) => new Dial { Integer = Integer };
        public static implicit operator Dial(string String) => new Dial { String = String };
    }

    public partial struct Iso4217CurrencyMinorUnitUnion
    {
        public Rules.Iso4217CurrencyMinorUnitEnum? Enum;
        public long? Integer;

        public static implicit operator Iso4217CurrencyMinorUnitUnion(Rules.Iso4217CurrencyMinorUnitEnum Enum) => new Iso4217CurrencyMinorUnitUnion { Enum = Enum };
        public static implicit operator Iso4217CurrencyMinorUnitUnion(long Integer) => new Iso4217CurrencyMinorUnitUnion { Integer = Integer };
    }

    public partial class Country
    {
        public static Country[] FromJson(string json) => JsonConvert.DeserializeObject<Country[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Country[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}