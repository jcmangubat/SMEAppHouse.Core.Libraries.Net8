using SMEAppHouse.ScraperBox.Puppeteer.Base;
using SMEAppHouse.ScraperBox.Puppeteer.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SMEAppHouse.ScraperBox.Puppeteer.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // test to get google.com content

            var options = new CrawlerOptionsPuppeteer()
            {
                IsHeadless = false,
            };
            ContentCrawlerViaPuppeteer crawler = new ContentCrawlerViaPuppeteer(options);
            Task.Run(() => crawler.WaitWhileNotReadyAsync()).Wait();

            //var htmlContent = await Task.Run(async () => { return await crawlerResult.HtmlPage.HtmlContentAsync(); });
            //var taskResult = crawler.GrabPage("http://yellowpages.com.au/"); 
            var taskResult = crawler.GrabPage("http://google.com");

            if (!taskResult.HasFailed)
            {
            }

            Console.ReadLine();

        }


        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                //WorkEnvelope.Instance.CrawlerAgent.Browser.PagesAsync().Result.ForEach(pg =>
                //{
                //    pg.CloseAsync().Wait();
                //});
                //WorkEnvelope.Instance.CrawlerAgent.Browser.CloseAsync().Wait();
            }
            catch (Exception exception)
            {
            }

        }
    }
}
