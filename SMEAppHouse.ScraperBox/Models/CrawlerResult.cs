using System;
using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Common.Models
{
    public class CrawlerResult : ICrawlerResult
    {
        public bool HasFailed { get; set; }
        public Exception CrawlerException { get; set; }
        public string PageContent { get; set; }
    }
}