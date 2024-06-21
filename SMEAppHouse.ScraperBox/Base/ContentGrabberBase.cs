using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Common.Base
{
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public abstract class ContentCrawlerBase<TCrawlerOptions, TCrawlerResult>
        : IContentCrawler<TCrawlerOptions, TCrawlerResult>
            where TCrawlerOptions : ICrawlerOptions
            where TCrawlerResult : ICrawlerResult
    {
        public bool IsReady { get; protected set; }
        public TCrawlerOptions CrawlerOptions { get; set; }

        protected ContentCrawlerBase(TCrawlerOptions crawlerOptions)
        {
            CrawlerOptions = crawlerOptions;
        }

        /// <summary>
        /// Ffetch the data from the URL as binary data and convert that to base64. This assumes the image
        /// always will be a JPEG. If it could sometimes be a different content type, you may well want to
        /// fetch the response as an HttpResponse and use that to propagate the content type.
        /// For future improvement, we may also want to add caching here as well.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<string> GetImageAsBase64Url(string url, NetworkCredential credentials = null)
        {
            using (var handler = new HttpClientHandler { Credentials = credentials })
            {
                using (var client = new HttpClient(handler))
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    return "image/jpeg;base64," + Convert.ToBase64String(bytes);
                }
            }
        }

        public event EventHandlers.ContentCrawlerGrabbingEventHandler<TCrawlerOptions> OnContentCrawlerGrabbing;
        public event EventHandlers.ContentCrawlerDoneEventHandler<TCrawlerResult> OnContentCrawlerDone;

        public async Task PerformRetry<TException>(Func<Task> operation, int maxRetryAttempts = 3)
            where TException : Exception
        {
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);
            await RetryCodeKit.RetryOnExceptionAsync<TException>(maxRetryAttempts, pauseBetweenFailures, operation);
        }

        public abstract void Dispose();

        public abstract TCrawlerResult GrabPage(string url);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task WaitWhileNotReadyAsync()
        {
            while (!IsReady)
            {
                await Task.Delay(25);
            }
        }
    }
}