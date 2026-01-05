using SMEAppHouse.Core.FreeIPProxy.Handlers;
using SMEAppHouse.Core.FreeIPProxy.Providers;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.FreeIPProxy.Services;
using System;
using System.Linq;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.YelloPgSpider.POC
{
    class Program
    {
        private static IPProxyManager _proxyManager;
        //private static int _fetched = 0;

        private static void Main(string[] args)
        {
            _proxyManager = new IPProxyManager(
                true,
                5,
                new IIPProxyCartridge[]
                {
                    new ProxyHttpNetCartridge(),
                    new PremProxyComCartridge()
                }
            );

            _proxyManager.OnFreeIPProxiesReading += ProxyManager_OnFreeIPProxiesReading;
            _proxyManager.OnFreeIPProxiesParsed += ProxyManager_OnFreeIPProxiesParsed;
            _proxyManager.OnIPProxyReady += _proxyManager_OnIPProxyReady;
            _proxyManager.OnIPProxyChecked += ProxyManager_OnIPProxyChecked;
            _proxyManager.OnIPProxiesChecked += _proxyManager_OnIPProxiesChecked;
            _proxyManager.Start();

            Console.ReadLine();
        }

        private static void ProxyManager_OnFreeIPProxiesParsed(object sender, EventHandlers.FreeIPProxiesParsedEventArgs e)
        {
            //_fetched += e.IPProxies.Count;
            Console.WriteLine($"Done reading from {e.TargetPageUrl}... Gave {e.IPProxies.Count} to bucket [{_proxyManager.ProxyBucket.Count}]");
            _ = ((IIPProxyCartridge)sender).IPProxies; // test the content of the acquired IP proxies.
        }

        private static void ProxyManager_OnFreeIPProxiesReading(object sender, EventHandlers.FreeIPProxiesReadingEventArgs e)
        {
            Console.WriteLine($"Now reading from {e.TargetPageUrl}...");
        }

        private static void ProxyManager_OnIPProxyChecked(object sender, EventHandlers.IPProxyCheckedEventArgs e)
        {
            Console.WriteLine($"IP proxy is {(e.IsValid ? "Valid" : "Invalid")} > {e.IPProxy.ToString()}");
        }

        private static void _proxyManager_OnIPProxiesChecked(object sender, EventHandlers.IPProxiesCheckedEventArgs e)
        {
            Console.WriteLine($"Proxies checked. {e.RemovedProxies} removed. [ Bucket: {_proxyManager.ProxyBucket.Count} Valid:{_proxyManager.ProxyBucket.Count(p => p.CheckStatus == IPProxy.CheckStatusEnum.Checked)} ]");
        }

        private static void _proxyManager_OnIPProxyReady(object sender, EventArgs e)
        {
            Console.WriteLine($"Proxy bucket is ready.");
        }
    }
}
