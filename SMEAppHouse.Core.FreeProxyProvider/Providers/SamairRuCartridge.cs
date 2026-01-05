using HtmlAgilityPack;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using static SMEAppHouse.Core.ScraperBox.Models.PageInstruction;

#pragma warning disable 168

namespace SMEAppHouse.Core.FreeIPProxy.Providers
{
    public class SamairRuCartridge : IPProxyCartridgeBase
    {
        public SamairRuCartridge(int startPageNo = 0)
            : base("http://samair.ru/proxy/proxy-{PAGENO}.htm",
                  new PageInstruction()
                  {
                      PadCharacter = '0',
                      PadLength = 2,
                      PaddingDirection = PaddingDirectionsEnum.ToLeft
                  },
                  startPageNo)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void ValidatePage(string content)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var document = doc.DocumentNode;
                var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "id", "proxylist");
                PageIsValid = target != null;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void ParseProxyPage(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var document = doc.DocumentNode;
            var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "id", "proxylist");

            if (target == null) return;

            var lines = ScraperBox.Helper.GetNodeCollection(target, "tr");

            lines.ToList().ForEach(e =>
            {
                if (e.Descendants("td").Count() <= 1) return;

                try
                {
                    var proxy = new IPProxy()
                    {
                        ProviderId = GetType().Name.Replace("Cartridge", ""),
                    };
                    var cells = e.Descendants("td").ToArray();

                    var unwanted = cells[0].Descendants("script");
                    var enumerable = unwanted as HtmlNode[] ?? unwanted.ToArray();

                    if (unwanted != null && enumerable.ToArray().Any())
                        enumerable.ToArray()[0].Remove();

                    var address = cells[0];

                    var addressParts = address.InnerText.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    proxy.IPAddress = addressParts[0].Trim();
                    proxy.PortNo = int.Parse(addressParts[1].Trim());

                    // get anonymity level
                    proxy.AnonymityLevel = cells[1].InnerText.Contains("high")
                        ? IPProxyRules.ProxyAnonymityLevelsEnum.Anonymous
                        : IPProxyRules.ProxyAnonymityLevelsEnum.Elite;

                    // get last checked time
                    var checkdate = cells[2].InnerText;
                    // Note: LastChecked will be set to current time if parsing fails
                    try
                    {
                        proxy.LastChecked = DateTime.Parse(checkdate);
                    }
                    catch
                    {
                        proxy.LastChecked = DateTime.Now;
                    }

                    // get the country
                    var countryPartial = cells[3].InnerText.ToLower().Replace(" ", "_");
                    countryPartial = countryPartial.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    proxy.Country = ScraperBox.Helper.FindProxyCountryFromPartial(countryPartial);

                    RegisterProxy(proxy);
                }
                catch (Exception exception)
                {
                    // Skip invalid proxy entries
                    Console.WriteLine($"Error parsing proxy: {exception.Message}");
                }

            });

            base.ParseProxyPage(content);
        }

    }
}

