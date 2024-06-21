using System;
using System.Collections.Generic;
using System.Linq;

namespace SMEAppHouse.ScraperBox.Common
{
    public class Rules
    {
        public enum HttpMethodsEnum
        {
            // ReSharper disable InconsistentNaming
            GET,
            POST,
            PUT,
            HEAD,
            TRACE,
            DELETE,
            SEARCH,
            CONNECT,
            PROPFIND,
            PROPPATCH,
            PATCH,
            MKCOL,
            COPY,
            MOVE,
            LOCK,
            UNLOCK,

            OPTIONS
            // ReSharper restore InconsistentNaming
        }

        public enum ContentTypesEnum
        {
            Xml,
            Json
        }

        public enum CountriesEnum
        {
            AF, //Afghanistan
            AL, //Albania
            AD, //Andorra
            AO, //Angola
            AR, //Argentina
            AM, //Armenia
            AU, //Australia
            AT, //Austria
            AZ, //Azerbaijan
            BD, //Bangladesh
            BY, //Belarus
            BE, //Belgium
            BJ, //Benin
            BO, //Bolivia
            BA, //Bosnia and Herzegovina
            BW, //Botswana
            BR, //Brazil
            BG, //Bulgaria
            BF, //Burkina Faso
            BI, //Burundi
            KH, //Cambodia
            CM, //Cameroon
            CA, //Canada
            CL, //Chile
            CN, //China
            CO, //Colombia
            CD, //Congo
            CR, //Costa Rica
            HR, //Croatia
            CY, //Cyprus
            CZ, //Czech
            DK, //Denmark
            DO, //Dominican Republic
            EC, //Ecuador
            EG, //Egypt
            EU, //Europe
            FI, //Finland
            FR, //France
            GA, //Gabon
            GE, //Georgia
            DE, //Germany
            GH, //Ghana
            GR, //Greece
            GT, //Guatemala
            HN, //Honduras
            HK, //Hong Kong
            HU, //Hungary
            IN, //India
            ID, //Indonesia
            IR, //Iran
            IQ, //Iraq
            IE, //Ireland
            IL, //Israel
            IT, //Italy
            JP, //Japan
            KZ, //Kazakhstan
            KE, //Kenya
            KR, //Korea
            KW, //Kuwait
            KG, //Kyrgyzstan
            LA, //Laos
            LV, //Latvia
            LB, //Lebanon
            LT, //Lithuania
            MO, //Macao
            MK, //Macedonia
            MG, //Madagascar
            MW, //Malawi
            MY, //Malaysia
            MX, //Mexico
            MD, //Moldova
            MN, //Mongolia
            ME, //Montenegro
            MZ, //Mozambique
            MM, //Myanmar
            NP, //Nepal
            NL, //Netherlands
            NZ, //New Zealand
            NI, //Nicaragua
            NG, //Nigeria
            NO, //Norway
            PK, //Pakistan
            PS, //Palestine
            PA, //Panama
            PY, //Paraguay
            PE, //Peru
            PH, //Philippines
            PL, //Poland
            PT, //Portugal
            PR, //Puerto Rico
            RO, //Romania
            RU, //Russia
            RS, //Serbia
            SG, //Singapore
            SK, //Slovakia
            SI, //Slovenia
            ZA, //South Africa
            ES, //Spain
            LK, //Sri Lanka
            SD, //Sudan
            SE, //Sweden
            CH, //Switzerland
            SY, //Syria
            TW, //Taiwan
            TH, //Thailand
            TR, //Turkey
            AE, //UAE
            UG, //Uganda
            UA, //Ukraine
            GB, //United Kingdom
            US, //United States
            UY, //Uruguay
            VE, //Venezuela
            VN, //Viet Nam
            ZW, //Zimbabwe 
        }

        public static List<Tuple<CountriesEnum, string>> IPProxyCountries =>
            new List<Tuple<CountriesEnum, string>>
            {
                new Tuple<CountriesEnum, string>(CountriesEnum.AF, "Afghanistan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AL, "Albania"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AD, "Andorra"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AO, "Angola"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AR, "Argentina"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AM, "Armenia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AU, "Australia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AT, "Austria"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AZ, "Azerbaijan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BD, "Bangladesh"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BY, "Belarus"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BE, "Belgium"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BJ, "Benin"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BO, "Bolivia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BA, "Bosnia and Herzegovina"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BW, "Botswana"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BR, "Brazil"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BG, "Bulgaria"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BF, "Burkina Faso"),
                new Tuple<CountriesEnum, string>(CountriesEnum.BI, "Burundi"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KH, "Cambodia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CM, "Cameroon"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CA, "Canada"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CL, "Chile"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CN, "China"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CO, "Colombia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CD, "Congo"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CR, "Costa Rica"),
                new Tuple<CountriesEnum, string>(CountriesEnum.HR, "Croatia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CY, "Cyprus"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CZ, "Czech"),
                new Tuple<CountriesEnum, string>(CountriesEnum.DK, "Denmark"),
                new Tuple<CountriesEnum, string>(CountriesEnum.DO, "Dominican Republic"),
                new Tuple<CountriesEnum, string>(CountriesEnum.EC, "Ecuador"),
                new Tuple<CountriesEnum, string>(CountriesEnum.EG, "Egypt"),
                new Tuple<CountriesEnum, string>(CountriesEnum.EU, "Europe"),
                new Tuple<CountriesEnum, string>(CountriesEnum.FI, "Finland"),
                new Tuple<CountriesEnum, string>(CountriesEnum.FR, "France"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GA, "Gabon"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GE, "Georgia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.DE, "Germany"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GH, "Ghana"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GR, "Greece"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GT, "Guatemala"),
                new Tuple<CountriesEnum, string>(CountriesEnum.HN, "Honduras"),
                new Tuple<CountriesEnum, string>(CountriesEnum.HK, "Hong Kong"),
                new Tuple<CountriesEnum, string>(CountriesEnum.HU, "Hungary"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IN, "India"),
                new Tuple<CountriesEnum, string>(CountriesEnum.ID, "Indonesia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IR, "Iran"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IQ, "Iraq"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IE, "Ireland"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IL, "Israel"),
                new Tuple<CountriesEnum, string>(CountriesEnum.IT, "Italy"),
                new Tuple<CountriesEnum, string>(CountriesEnum.JP, "Japan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KZ, "Kazakhstan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KE, "Kenya"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KR, "Korea"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KW, "Kuwait"),
                new Tuple<CountriesEnum, string>(CountriesEnum.KG, "Kyrgyzstan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.LA, "Laos"),
                new Tuple<CountriesEnum, string>(CountriesEnum.LV, "Latvia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.LB, "Lebanon"),
                new Tuple<CountriesEnum, string>(CountriesEnum.LT, "Lithuania"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MO, "Macao"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MK, "Macedonia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MG, "Madagascar"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MW, "Malawi"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MY, "Malaysia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MX, "Mexico"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MD, "Moldova"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MN, "Mongolia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.ME, "Montenegro"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MZ, "Mozambique"),
                new Tuple<CountriesEnum, string>(CountriesEnum.MM, "Myanmar"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NP, "Nepal"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NL, "Netherlands"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NZ, "New Zealand"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NI, "Nicaragua"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NG, "Nigeria"),
                new Tuple<CountriesEnum, string>(CountriesEnum.NO, "Norway"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PK, "Pakistan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PS, "Palestine"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PA, "Panama"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PY, "Paraguay"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PE, "Peru"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PH, "Philippines"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PL, "Poland"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PT, "Portugal"),
                new Tuple<CountriesEnum, string>(CountriesEnum.PR, "Puerto Rico"),
                new Tuple<CountriesEnum, string>(CountriesEnum.RO, "Romania"),
                new Tuple<CountriesEnum, string>(CountriesEnum.RU, "Russia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.RS, "Serbia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SG, "Singapore"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SK, "Slovakia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SI, "Slovenia"),
                new Tuple<CountriesEnum, string>(CountriesEnum.ZA, "South Africa"),
                new Tuple<CountriesEnum, string>(CountriesEnum.ES, "Spain"),
                new Tuple<CountriesEnum, string>(CountriesEnum.LK, "Sri Lanka"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SD, "Sudan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SE, "Sweden"),
                new Tuple<CountriesEnum, string>(CountriesEnum.CH, "Switzerland"),
                new Tuple<CountriesEnum, string>(CountriesEnum.SY, "Syria"),
                new Tuple<CountriesEnum, string>(CountriesEnum.TW, "Taiwan"),
                new Tuple<CountriesEnum, string>(CountriesEnum.TH, "Thailand"),
                new Tuple<CountriesEnum, string>(CountriesEnum.TR, "Turkey"),
                new Tuple<CountriesEnum, string>(CountriesEnum.AE, "UAE"),
                new Tuple<CountriesEnum, string>(CountriesEnum.UG, "Uganda"),
                new Tuple<CountriesEnum, string>(CountriesEnum.UA, "Ukraine"),
                new Tuple<CountriesEnum, string>(CountriesEnum.GB, "United Kingdom"),
                new Tuple<CountriesEnum, string>(CountriesEnum.US, "United States"),
                new Tuple<CountriesEnum, string>(CountriesEnum.UY, "Uruguay"),
                new Tuple<CountriesEnum, string>(CountriesEnum.VE, "Venezuela"),
                new Tuple<CountriesEnum, string>(CountriesEnum.VN, "Viet Nam"),
                new Tuple<CountriesEnum, string>(CountriesEnum.ZW, "Zimbabwe"),
            };

        public static Tuple<CountriesEnum, string> FindCountry(string countryPartial)
        {
            return IPProxyCountries.FirstOrDefault(c => c.Item2.Contains(countryPartial));
        }

        public enum ProxyAnonymityLevelsEnum
        {
            /// <summary>
            /// Level 1 - Highly Anonymous Proxy: The web server can't detect whether you are using a proxy
            /// </summary>
            Elite,

            /// <summary>
            /// Level 2 - Anonymous Proxy: The web server can know you are using a proxy, but it can't know your real IP.
            /// </summary>
            Anonymous,

            /// <summary>
            /// Level 3 - Transparent Proxy: The web server can know you are using a proxy and it can also know your real IP.
            /// </summary>
            Transparent
        }

        /// <summary>
        /// Secure websites whose url starts with https:// instead of http://
        /// (ex. https://www.paypal.com) use the encrypted (SSL/HTTPS) connections
        /// between its web server and the visitors. Some proxies only support ordinary
        /// http sites and can't surf https sites.
        /// Elite Proxy Switcher can test whether a proxy supports https sites.
        /// </summary>
        public enum ProxyWebProtocolsEnum
        {
            // ReSharper disable once InconsistentNaming
            HTTP,
            HTTPS,
            SOCKS4_5
        }

        public enum ProxyCheckerStatusEnum
        {
            NotChecked,
            Checking,
            Checked,
            CheckedInvalid,
        }

    }

}
