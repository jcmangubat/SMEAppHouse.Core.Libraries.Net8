using System;
using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Base
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

        // ContentGenerator Event Handlers
        public class StartingEventArgs : EventArgs
        {
            public Models.HtmlTarget Target { get; set; }
            public bool Cancel { get; set; }

            public StartingEventArgs(Models.HtmlTarget target)
            {
                Target = target;
            }
        }
        public delegate void StartingEventHandler(object sender, StartingEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            public Models.IPProxy FreeProxy { get; set; }
            public Models.HtmlTarget Target { get; set; }
            public TimeSpan ElapsedTime { get; set; }
            
            public DoneEventArgs()
                : this(null)
            {
            }

            public DoneEventArgs(Models.HtmlTarget target)
                : this(target, new TimeSpan())
            {

            }

            public DoneEventArgs(Models.HtmlTarget target, TimeSpan elapsedTime)
                : this(target, null, elapsedTime)
            {

            }

            public DoneEventArgs(Models.HtmlTarget target, Models.IPProxy freeProxy, TimeSpan elapsedTime)
            {
                FreeProxy = freeProxy;
                ElapsedTime = elapsedTime;
                Target = target;
            }
        }
        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class OperationExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; set; }

            public OperationExceptionEventArgs(Exception exception)
            {
                Exception = exception;
            }
        }
        public delegate void OperationExceptionEventHandler(object sender, OperationExceptionEventArgs e);
    }
}

