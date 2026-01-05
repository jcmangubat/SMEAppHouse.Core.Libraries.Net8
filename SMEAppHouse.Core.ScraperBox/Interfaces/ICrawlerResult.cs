using System;

namespace SMEAppHouse.Core.ScraperBox.Interfaces
{
    public interface ICrawlerResult
    {
        bool HasFailed { get; set; }
        Exception CrawlerException { get; set; }
        string PageContent { get; set; }
    }
}

