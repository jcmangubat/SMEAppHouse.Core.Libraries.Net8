using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;
using SMEAppHouse.ScraperBox.Common.Base;
using SMEAppHouse.ScraperBox.Puppeteer.Interface;
using SMEAppHouse.ScraperBox.Puppeteer.Models;

namespace SMEAppHouse.ScraperBox.Puppeteer.Base
{
    public sealed class ContentCrawlerViaPuppeteer :
        ContentCrawlerBase<ICrawlerOptionsPuppeteer, ICrawlerResultPuppeteer>,
        IContentCrawlerViaPuppeteer<ICrawlerOptionsPuppeteer, ICrawlerResultPuppeteer>
    {
        public Browser Browser { get; set; }

        public ContentCrawlerViaPuppeteer(ICrawlerOptionsPuppeteer options)
            : base(options)
        {
            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Attempting to update the chromium engine browser to local...");

                await (new BrowserFetcher())
                    .DownloadAsync(BrowserFetcher.DefaultRevision);
                Console.WriteLine("Navigating to developers.google.com");

                // Args = new string[] { "--proxy-server='direct://'",
                //                       "--proxy-bypass-list=*" }

                var browserArgs = new List<string>();
                if (CrawlerOptions.NoSandbox) browserArgs.Add("--no-sandbox");
                if (CrawlerOptions.No2DCanvas) browserArgs.Add("--disable-accelerated-2d-canvas");
                if (CrawlerOptions.NoGPU) browserArgs.Add("--disable-gpu");

                if (CrawlerOptions.UseProxy && !string.IsNullOrEmpty(CrawlerOptions.IPProxy.ToString()))
                    browserArgs.Add($"--proxy-server={CrawlerOptions.IPProxy.ToString()}");

                Browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = CrawlerOptions.IsHeadless,
                    Args = browserArgs.ToArray()
                });

                IsReady = true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public override ICrawlerResultPuppeteer GrabPage(string url)
        {
            if (!IsReady)
                throw new Exception("Chromium instance is not ready yet.");

            var ccgEvArg = new EventHandlers.ContentCrawlerGrabbingEventArgs<ICrawlerOptionsPuppeteer>(url, CrawlerOptions);

            var crawlerResult = new CrawlerResultPuppeteer()
            {
                Url = url
            };

            Task.Run(async () =>
            {
                var pages = await Browser.PagesAsync();
                if (pages.FirstOrDefault() != null)
                    crawlerResult.HtmlPage = pages.LastOrDefault();

                if (!CrawlerOptions.InNewPage && crawlerResult.HtmlPage.Url != "about:blank")
                    crawlerResult.HtmlPage = await Browser.NewPageAsync();

                if (crawlerResult.HtmlPage == null)
                    throw new Exception("Cannot create new page.");

                if (!CrawlerOptions.IsHeadless)
                    await crawlerResult.HtmlPage.SetViewportAsync(new ViewPortOptions()
                    { Width = 1024, Height = 842 });

            }).Wait();

            Task.Run(async () =>
            {
                if (CrawlerOptions.NoImage)
                {
                    await crawlerResult.HtmlPage.SetRequestInterceptionAsync(true);

                    // disable images to download
                    crawlerResult.HtmlPage.Request += (sender, e) =>
                    {
                        if (e.Request.ResourceType == ResourceType.Image)
                            e.Request.AbortAsync().Wait();
                        else
                            e.Request.ContinueAsync().Wait();
                    };

                }
            }).Wait();

            var checkError = string.Empty;

            Task.Run(async () =>
            {
                var catContTsk = await crawlerResult.HtmlPage.GoToAsync(url, WaitUntilNavigation.Networkidle2);
                const string jsCode = @"() => {
const selector = document.querySelector('[class=""error-404""]'); 
return selector ? (selector.innerHTML || '') : '';
}";
                var checkErrorTsk = crawlerResult.HtmlPage.EvaluateFunctionAsync<string>(jsCode);
                checkError = checkErrorTsk.Result;
            }).Wait();


            if (string.IsNullOrEmpty(checkError))
            {
                Task.Run(async () =>
                {
                    var htmlContentTask = await crawlerResult.HtmlPage.HtmlContentAsync();
                    crawlerResult.PageContent = htmlContentTask;
                    crawlerResult.HtmlPage.SetRequestInterceptionAsync(false).Wait();
                }).Wait();
                crawlerResult.HtmlPage.Request -= null;
            }
            else
            {
                crawlerResult.CrawlerException = new Exception("Error");
                crawlerResult.HasFailed = true;
            }


            return crawlerResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Task.Factory.StartNew(async () =>
            {
                var pages = await Browser.PagesAsync();
                foreach (var page in pages)
                {
                    await page.CloseAsync();
                }
                await Browser.CloseAsync();
            }).Wait();
        }
    }
}