using System;
using SMEAppHouse.ScraperBox.FreeIPProxies.Providers;
using SMEAppHouse.ScraperBox.FreeIPProxies.Providers.Base;
using SMEAppHouse.ScraperBox.Puppeteer.Base;
using SMEAppHouse.ScraperBox.Puppeteer.Models;

//using FreeIPProxiesHandlers = SMEAppHouse.Core.FreeIPProxy.Handlers;

namespace SMEAppHouse.Core.FreeIPProxy.Test
{
    class Program
    {
        private static int _fetched = 0;
        //private static IPProxyManager _proxyManager = null;

        private static void Main(string[] args)
        {

            var contentCrawlerViaPuppeteer = new ContentCrawlerViaPuppeteer(new CrawlerOptionsPuppeteer() { IsHeadless = false });
            //var proxyHttpNetCartridge = new ProxyHttpNetCartridge(0);
            var premProxyComCartridge = new PremProxyComCartridge(contentCrawlerViaPuppetee);

            _proxyManager = new IPProxyManager(
                autoIpCheck: true,
                ipCheckWorkerCounts: 5,
                proxyCartridges: new IIPProxyCartridge[] { premProxyComCartridge });

            _proxyManager.OnFreeIPProviderSourcesCompleted += _proxyManager_OnFreeIPProviderSourcesCompleted;
            _proxyManager.OnFreeIPProviderSourceCompleted += _proxyManager_OnFreeIPProviderSourceCompleted;
            _proxyManager.OnFreeIPProxiesReading += ProxyManager_OnFreeIPProxiesReading;
            _proxyManager.OnFreeIPProxiesParsed += _proxyManager_OnFreeIPProxiesParsed;
            _proxyManager.OnIPProxyChecked += ProxyManager_OnIPProxyChecked;
            _proxyManager.OnIPProxyReady += _proxyManager_OnIPProxyReady;
            _proxyManager.OnFreeIPProxyParsed += _proxyManager_OnFreeIPProxyParsed;
            _proxyManager.Start();

            Console.ReadLine();
        }

        private static void _proxyManager_OnFreeIPProxiesParsed(object sender, FreeIPProxiesHandlers.EventHandlers.FreeIPProxiesParsedEventArgs e)
        {
            _fetched += e.IPProxies.Count;
            Console.WriteLine($"Done reading from {e.TargetPageUrl}... pushed to bucket: [{e.IPProxies.Count}]");
            var test = ((IIPProxyCartridge)sender).IPProxies;
        }

        private static void _proxyManager_OnFreeIPProxyParsed(object sender, FreeIPProxiesHandlers.EventHandlers.FreeIPProxyParsedEventArgs e)
        {
            Console.WriteLine($"Proxy parsed > {e.IPProxy.ToString()}");
        }

        private static void _proxyManager_OnFreeIPProviderSourceCompleted(object sender, FreeIPProxiesHandlers.EventHandlers.FreeIPProviderSourceCompletedEventArgs e)
        {
            Console.WriteLine($"Done scraping from {sender.GetType().Name}. Total pages scraped: {e.TotalPages}");
        }

        private static void _proxyManager_OnFreeIPProviderSourcesCompleted(object sender, FreeIPProxiesHandlers.EventHandlers.FreeIPProviderSourcesCompletedEventArgs e)
        {
            Console.WriteLine($"Done scraping from all sources. Total pages scraped: {e.TotalPages}");
        }


        private static void ProxyManager_OnFreeIPProxiesReading(object sender, FreeIPProxiesHandlers.EventHandlers.FreeIPProxiesReadingEventArgs e)
        {
            Console.WriteLine($"Now reading from {e.TargetPageUrl}...");
        }

        private static void ProxyManager_OnIPProxyChecked(object sender, FreeIPProxiesHandlers.EventHandlers.IPProxyCheckedEventArgs e)
        {
            Console.WriteLine($"IP checked is {(e.IsValid ? "Valid" : "Invalid")} > {e.IPProxy.ToString()} last checked: {e.IPProxy.LastChecked} [{_proxyManager.ProxyBucket.Count} of {_fetched}]");
        }

        private static void _proxyManager_OnIPProxyReady(object sender, EventArgs e)
        {
            Console.WriteLine($"Proxy bucket is ready.");
        }
    }
}
