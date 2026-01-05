using System;
using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Models
{
    public class CrawlerResult : ICrawlerResult
    {
        public bool HasFailed { get; set; }
        public Exception CrawlerException { get; set; }
        public string PageContent { get; set; }
    }
}

