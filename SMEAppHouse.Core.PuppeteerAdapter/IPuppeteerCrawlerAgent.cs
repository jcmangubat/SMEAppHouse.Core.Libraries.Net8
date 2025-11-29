using System.Net;
using PuppeteerSharp;

namespace SMEAppHouse.Core.PuppeteerAdapter
{
    public interface IPuppeteerCrawlerAgent : IDisposable
    {
        IBrowser Browser { get; set; }
        IPage HtmlPage { get; set; }

        bool IsReady { get; }
        bool IsHeadless { get; }

        bool TryGrabPage(string url, out Task<IResponse> responseTsk, bool inNewPage = false, bool? noImage = false);
        bool TryGrabPage(string url, out Task<IResponse> responseTsk, out IPage page, bool inNewPage = false, bool? noImage = false);
        Task<T> ExecuteJava<T>(string jsCode) where T : struct;
        Task PerformRetry<TException>(Func<Task> operation, int maxRetryAttempts = 3) where TException : Exception;
        Task<string> GetImageAsBase64Url(string url, NetworkCredential credentials = null);
    }
}