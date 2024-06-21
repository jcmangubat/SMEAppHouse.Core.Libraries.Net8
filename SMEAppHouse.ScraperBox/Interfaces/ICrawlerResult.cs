using System;

namespace SMEAppHouse.ScraperBox.Common.Interfaces
{
    public interface ICrawlerResult
    {
        bool HasFailed { get; set; }
        Exception CrawlerException { get; set; }
        string PageContent { get; set; }
    }
}