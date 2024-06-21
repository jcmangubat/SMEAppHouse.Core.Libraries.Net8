using SMEAppHouse.Core.ISOResource.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.ISOResource
{
    public class Country_x
    {
        /// <summary>
        /// Country or Area official Arabic short name from UN Statistics Divsion
        /// </summary>
        [ISOField("official_name_ar")]
        public string OfficialNameArabic { get; set; }

        /// <summary>
        /// Country or Area official Chinese short name from UN Statistics Divsion
        /// </summary>
        [ISOField("official_name_cn")]
        public string OfficialNameChinese { get; set; }

        /// <summary>
        /// Country or Area official English short name from UN Statistics Divsion
        /// </summary>
        [ISOField("official_name_en")]
        public string OfficialNameEnglish { get; set; }

        /// <summary>
        /// Country or Area official Spanish short name from UN Statistics Divsion
        /// </summary>
        [ISOField("official_name_es")]
        public string OfficialNameSpanish { get; set; }

        [ISOField("official_name_fr	5	string Country or Area official French short name from UN Statistics Divsion")]
        public string OfficialNameFrench { get; set; }

        [ISOField("official_name_ru	6	string Country or Area official Russian short name from UN Statistics Divsion")]
        public string OfficialNameRussian { get; set; }

        [ISOField("ISO3166-1-Alpha-2	7	string Alpha-2 codes from ISO 3166-1")]
        public string ISO3166Alpha2 { get; set; }

        [ISOField("ISO3166-1-Alpha-3	8	string Alpha-3 codes from ISO 3166-1 (synonymous with World Bank Codes)")]
        public string ISO3166Alpha3 { get; set; }

        [ISOField("ISO3166-1-numeric	9	string Numeric codes from ISO 3166-1")]
        public string ISO3166AlphaNumeric { get; set; }

        [ISOField("ISO4217-currency_alphabetic_code	10	string ISO 4217 currency alphabetic code")]
        public string ISO4217CurrencyAlphaCode { get; set; }

        [ISOField("ISO4217-currency_country_name	11	string ISO 4217 country name")]
        public string ISO4217CurrencyCountryMame { get; set; }

        [ISOField("ISO4217-currency_minor_unit	12	string ISO 4217 currency number of minor units")]
        public string ISO4217CurrencyMinorUnit { get; set; }

        [ISOField("ISO4217-currency_name	13	string ISO 4217 currency name")]
        public string ISO4217CurrencyName { get; set; }

        [ISOField("ISO4217-currency_numeric_code	14	string ISO 4217 currency numeric code")]
        public string ISO4217CurrencyNumericCode { get; set; }

        [ISOField("M49	15	number UN Statistics M49 numeric codes(nearly synonymous with ISO 3166-1 numeric codes, which are based on UN M49. ISO 3166-1 does not include Channel Islands or Sark, for example)")]
        public string M49 { get; set; }

        [ISOField("UNTERM Arabic Formal	16	string Country's formal Arabic name from UN Protocol and Liaison Service")]
        public string UNTERMArabicFormal { get; set; }
        [ISOField("UNTERM Arabic Short	17	string Country's short Arabic name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Chinese Formal	18	string Country's formal Chinese name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Chinese Short	19	string Country's short Chinese name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM English Formal	20	string Country's formal English name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM English Short	21	string Country's short English name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM French Formal	22	string Country's formal French name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM French Short	23	string Country's short French name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Russian Formal	24	string Country's formal Russian name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Russian Short	25	string Country's short Russian name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Spanish Formal	26	string Country's formal Spanish name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("UNTERM Spanish Short	27	string Country's short Spanish name from UN Protocol and Liaison Service")]
        public string OfficialNameArabic { get; set; }
        [ISOField("CLDR display name	28	string Country's customary English short name (CLDR)")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Capital	29	string Capital city from Geonames")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Continent	30	string Continent from Geonames")]
        public string OfficialNameArabic { get; set; }
        [ISOField("DS	31	string Distinguishing signs of vehicles in international traffic")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Developed / Developing Countries	32	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Dial	33	string Country code from ITU-T recommendation E.164, sometimes followed by area code")]
        public string OfficialNameArabic { get; set; }
        [ISOField("EDGAR	34	string EDGAR country code from SEC")]
        public string OfficialNameArabic { get; set; }
        [ISOField("FIFA	35	string Codes assigned by the Fédération Internationale de Football Association")]
        public string OfficialNameArabic { get; set; }
        [ISOField("FIPS	36	string Codes from the U.S.standard FIPS PUB 10-4")]
        public string OfficialNameArabic { get; set; }
        [ISOField("GAUL	37	string Global Administrative Unit Layers from the Food and Agriculture Organization")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Geoname ID	38	number Geoname ID")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Global Code	39	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Global Name	40	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("IOC	41	string Codes assigned by the International Olympics Committee")]
        public string OfficialNameArabic { get; set; }
        [ISOField("ITU	42	string Codes assigned by the International Telecommunications Union")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Intermediate Region Code	43	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Intermediate Region Name	44	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Land Locked Developing Countries(LLDC) 45	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Languages	46	string Languages from Geonames")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Least Developed Countries(LDC) 47	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("MARC	48	string MAchine-Readable Cataloging codes from the Library of Congress")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Region Code	49	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Region Name	50	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Small Island Developing States(SIDS)   51	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Sub-region Code	52	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("Sub-region Name	53	string Country classification from United Nations Statistics Division")]
        public string OfficialNameArabic { get; set; }
        [ISOField("TLD	54	string Top level domain from Geonames")]
        public string OfficialNameArabic { get; set; }
        [ISOField("WMO	55	string Country abbreviations by the World Meteorological Organization")]
        public string OfficialNameArabic { get; set; }
        [ISOField("is_independent	56	string Country status, based on the CIA World Factbook")]
        public string OfficialNameArabic { get; set; }

        public class Countries : List<Country>
        {
        }
    }
}
