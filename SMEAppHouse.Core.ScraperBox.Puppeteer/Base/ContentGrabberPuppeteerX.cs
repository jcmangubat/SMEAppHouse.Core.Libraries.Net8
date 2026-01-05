using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;
using SMEAppHouse.Core.ScraperBox.Base;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Base
{
    public sealed class ContentGrabberPuppeteerX : ContentGrabberBase<Response, Page>, IContentGrabberPuppeteer
    {
        public Browser Browser { get; set; }
        public Page HtmlPage { get; set; }
        public bool IsHeadless { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useIPProxyIfAny"></param>
        /// <param name="headless"></param>
        /// <param name="noSandbox"></param>
        /// <param name="no2DCanvas"></param>
        /// <param name="noGPU"></param>
        public ContentGrabberPuppeteer(bool useIPProxyIfAny = false,
            bool headless = true,
            bool? noSandbox = null,
            bool? no2DCanvas = null,
            bool? noGPU = null) : base(useIPProxyIfAny)
        {
            IsHeadless = headless;
            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Attempting to update the chromium engine browser to local...");

                await (new BrowserFetcher())
                    .DownloadAsync(BrowserFetcher.DefaultRevision);
                Console.WriteLine("Navigating to developers.google.com");

                // Args = new string[] { "--proxy-server='direct://'",
                //                       "--proxy-bypass-list=*" }

                var browserArgs = new List<string>();
                if (noSandbox.HasValue && noSandbox.Value) browserArgs.Add("--no-sandbox");
                if (no2DCanvas.HasValue && no2DCanvas.Value) browserArgs.Add("--disable-accelerated-2d-canvas");
                if (noGPU.HasValue && noGPU.Value) browserArgs.Add("--disable-gpu");

                if (base.UseProxy && !string.IsNullOrEmpty(base.IPProxy.ToString()))
                    browserArgs.Add($"--proxy-server={IPProxy}");

                Browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = IsHeadless,
                    Args = browserArgs.ToArray()
                });

                var pages = await Browser.PagesAsync();
                if (pages.FirstOrDefault() != null)
                    HtmlPage = pages.FirstOrDefault();
                else HtmlPage = await Browser.NewPageAsync();

                if (HtmlPage == null)
                    throw new Exception("Cannot create new page.");

                if (!IsHeadless)
                    await HtmlPage.SetViewportAsync(new ViewPortOptions() { Width = 1024, Height = 842 });

                IsReady = true;
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsCode"></param>
        /// <returns></returns>
        public async Task<T> ExecuteJava<T>(string jsCode)
            where T : struct
        {
            return await HtmlPage.EvaluateFunctionAsync<T>(jsCode);
            // TODO: add await selector here!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseTsk"></param>
        /// <param name="docContent"></param>
        /// <param name="inNewPage"></param>
        /// <param name="noImage"></param>
        /// <returns></returns>
        public override Page TryGrabContent(string url, out Task<Response> responseTsk, out string docContent, bool inNewPage = false, bool? noImage = false)
        {
            if (!IsReady)
                throw new Exception("Chromium instance is not ready yet.");

            var pg = HtmlPage;

            if (inNewPage)
                Task.Run(async () =>
                {
                    pg = await Browser.NewPageAsync();
                    if (!IsHeadless)
                        await pg.SetViewportAsync(new ViewPortOptions() { Width = 1024, Height = 842 });
                }).Wait();

            Task.Run(async () =>
            {
                if (noImage.HasValue && noImage.Value)
                {
                    await pg.SetRequestInterceptionAsync(true);

                    // disable images to download
                    pg.Request += (sender, e) =>
                    {
                        if (e.Request.ResourceType == ResourceType.Image)
                            e.Request.AbortAsync().Wait();
                        else
                            e.Request.ContinueAsync().Wait();
                    };

                }
            }).Wait();


            var catContTsk = pg.GoToAsync(url, WaitUntilNavigation.Networkidle2);
            catContTsk.Wait();

            var jsCode = @"() => {
const selector = document.querySelector('[class=""error-404""]'); 
return selector ? (selector.innerHTML || '') : '';
}";
            var checkErrorTsk = HtmlPage.EvaluateFunctionAsync<string>(jsCode);
            var checkError = checkErrorTsk.Result;
            responseTsk = catContTsk;
            var page = pg;

            docContent = page.HtmlContent();
            pg.SetRequestInterceptionAsync(false).Wait();
            pg.Request -= null;

            if (!string.IsNullOrEmpty(checkError))
                throw new Exception("Error");

            return page;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Task.Factory.StartNew(async () =>
            {
                await HtmlPage.CloseAsync();
                await Browser.CloseAsync();
            });
        }

    }
}