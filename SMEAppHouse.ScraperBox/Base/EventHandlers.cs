using System;
using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Common.Base
{
    public class EventHandlers
    {
        public class ContentCrawlerGrabbingEventArgs<TCrawlerOptions> : EventArgs
            where TCrawlerOptions : ICrawlerOptions
        {
            public string PageUrl { get; private set; }
            public TCrawlerOptions CrawlerOptions { get; set; }

            public ContentCrawlerGrabbingEventArgs(string pageUrl, TCrawlerOptions options)
            {
                PageUrl = pageUrl;
                CrawlerOptions = options;
            }
        }

        public delegate void ContentCrawlerGrabbingEventHandler<TCrawlerOptions>(object sender,
            ContentCrawlerGrabbingEventArgs<TCrawlerOptions> e)
            where TCrawlerOptions : ICrawlerOptions;

        public class ContentCrawlerDoneEventArgs<TCrawlerResult> : EventArgs
            where TCrawlerResult : ICrawlerResult
        {
            public string PageUrl { get; private set; }
            public TCrawlerResult CrawlerResult { get; set; }

            public ContentCrawlerDoneEventArgs(string pageUrl, TCrawlerResult result)
            {
                PageUrl = pageUrl;
                CrawlerResult = result;
            }
        }

        public delegate void ContentCrawlerDoneEventHandler<TCrawlerResult>(object sender,
            ContentCrawlerDoneEventArgs<TCrawlerResult> e)
            where TCrawlerResult : ICrawlerResult;


    }
}