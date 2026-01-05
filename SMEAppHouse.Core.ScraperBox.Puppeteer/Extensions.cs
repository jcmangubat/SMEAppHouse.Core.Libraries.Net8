using System;
using System.Globalization;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlPage"></param>
        /// <returns></returns>
        public static async Task<string> HtmlContentAsync(this Page htmlPage)
        {
            try
            {
                //Issue: https://github.com/GoogleChrome/puppeteer/issues/331
                //const string jsCode = @"async () => {
                //const bodyHTML = await page.evaluate(() => document.body.innerHTML); 
                //return bodyHTML;
                //}";

                ////const bodyHtml = await page.content();
                ///
                const string jsCode = @"async function getHtmlContent() {                    
                    const bodyHTML = await page.evaluate(() => 'gotcha!'); 
                    return bodyHtml;
                }";

                //var val = await Task.Run(() => RunLongTask(i.ToString(CultureInfo.InvariantCulture)));
                //var content = await htmlPage.EvaluateFunctionAsync<string>(jsCode);

                var content = await Task.Run(() => htmlPage.EvaluateFunctionAsync<string>(jsCode));
                return content;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlPage"></param>
        /// <param name="jsCode"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteJavaAsync<T>(this Page htmlPage, string jsCode) where T : struct
        {
            return await htmlPage.EvaluateFunctionAsync<T>(jsCode);

            //var ccgEvArg = new EventHandlers.ContentCrawlerGrabbingEventArgs<ICrawlerOptionsPuppeteer>("", CrawlerOptions);

            /*TODO: add await selector here!*/
        }

    }
}