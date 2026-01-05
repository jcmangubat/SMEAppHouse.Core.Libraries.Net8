using System;
using System.Net;
using System.Threading.Tasks;
using SMEAppHouse.Core.ScraperBox.Base;

namespace SMEAppHouse.Core.ScraperBox.Interfaces
{
    public interface IContentCrawler<TCrawlerOptions, TCrawlerResult> : IDisposable
        where TCrawlerOptions : ICrawlerOptions
        where TCrawlerResult : ICrawlerResult
    {
        TCrawlerOptions CrawlerOptions { get; set; }

        bool IsReady { get; }

        Task PerformRetry<TException>(Func<Task> operation, int maxRetryAttempts = 3) where TException : Exception;
        Task<string> GetImageAsBase64Url(string url, NetworkCredential credentials = null);

        event EventHandlers.ContentCrawlerGrabbingEventHandler<TCrawlerOptions> OnContentCrawlerGrabbing;
        event EventHandlers.ContentCrawlerDoneEventHandler<TCrawlerResult> OnContentCrawlerDone;

        TCrawlerResult GrabPage(string url);
    }
}

