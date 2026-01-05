using System;
using System.Linq;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.ScraperBox.FreeIPProxies
{
    public class IPProxyRules
    {
        /// <summary>
        /// Secure websites whose url starts with https:// instead of http://
        /// (ex. https://www.paypal.com) use the encrypted (SSL/HTTPS) connections
        /// between its web server and the visitors. Some proxies only support ordinary
        /// http sites and can't surf https sites.
        /// Elite Proxy Switcher can test whether a proxy supports https sites.
        /// </summary>
        public enum ProxyWebProtocolsEnum
        {
            HTTP,
            HTTPS,
            // ReSharper disable once InconsistentNaming
            SOCKS4_5
        }

        /// <summary>
        /// Expresses 3 levels of proxies according to their anonymity.
        /// </summary>
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

        public enum ProxySpeedsEnum
        {
            Slow,
            Medium,
            Fast
        }

        public enum ProxyConnectionSpeedsEnum
        {
            Slow,
            Medium,
            Fast
        }
    }

    public class Rules
    {
        public enum ProxyCheckerStatusEnum
        {
            NotChecked,
            Checking,
            Checked,
            CheckedInvalid
        }

        public enum CountriesEnum
        {
            // This will be populated from Rules.WorldCountriesEnum
        }

        public static Tuple<object, string> FindCountry(string countryPartial)
        {
            var country = SMEAppHouse.Core.ScraperBox.Helper.FindProxyCountryFromPartial(countryPartial);
            if (country != null)
            {
                return new Tuple<object, string>(country, country.ToString());
            }
            return null;
        }
    }
}
