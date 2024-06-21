using System;
using System.Net;

namespace SMEAppHouse.ScraperBox.Common.Models
{
    [Serializable]
    public class IPProxy
    {
        public Guid Id { get; private set; }
        public string ProviderId { get; set; }

        public string IPAddress { get; set; }
        public int PortNo { get; set; }

        public Rules.ProxyWebProtocolsEnum Protocol { get; set; }
        public Rules.ProxyAnonymityLevelsEnum Anonymity { get; set; }
        public Rules.CountriesEnum Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public double UpTime { get; set; }

        public int ResponseRate { get; set; }
        public int SpeedRate { get; set; }

        public double ResponseScore { get; set; }
        public double TransferScore { get; set; }

        public DateTime LastChecked { get; set; }

        public string ISP { get; set; }

        public Guid CheckerTokenId { get; set; }

        public Rules.ProxyCheckerStatusEnum CheckStatus { get; set; }

        public Tuple<string, string> Credential { get; set; }

        public NetworkCredential GetNetworkCredential()
        {
            return this.Credential != null
                ? new NetworkCredential(this.Credential.Item1, this.Credential.Item2)
                : null;
        }

        public IPProxy()
        {
            Id = Guid.NewGuid();
        }

        public Tuple<string, string> AsTuple()
        {
            return new Tuple<string, string>(this.IPAddress, this.PortNo.ToString());
        }

        public override string ToString()
        {
            return $"{IPAddress}:{PortNo}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IWebProxy ToWebProxy()
        {
            return new WebProxy(IPAddress, PortNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetLastValidationElapsedTime()
        {
            var lastValidationElapsedTime = DateTime.Now.Subtract(LastChecked);
            return lastValidationElapsedTime;
        }

        
    }
}
