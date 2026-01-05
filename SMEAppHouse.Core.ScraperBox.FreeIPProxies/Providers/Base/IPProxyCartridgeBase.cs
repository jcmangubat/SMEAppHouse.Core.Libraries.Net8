using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SMEAppHouse.Core.CodeKits.Data;
using SMEAppHouse.Core.ProcessService.Engines;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Base;
using SMEAppHouse.Core.ScraperBox.Interfaces;
using SMEAppHouse.Core.ScraperBox.Models;
using SMEAppHouse.Core.ScraperBox.FreeIPProxies.Handlers;
using SMEAppHouse.Core.ScraperBox.Puppeteer;
using SMEAppHouse.Core.ScraperBox.Puppeteer.Base;
using EventHandlers = SMEAppHouse.Core.ScraperBox.FreeIPProxies.Handlers.EventHandlers;

namespace SMEAppHouse.Core.ScraperBox.FreeIPProxies.Providers.Base
{
    public class IPProxyCartridgeBase<TContentCrawler> : ProcessAgentViaThread, IIPProxyCartridge
        where TContentCrawler : IContentCrawler<ICrawlerOptions, ICrawlerResult>
    {
        #region private and protected properties

        private readonly TContentCrawler _contentCrawler = default(TContentCrawler);

        protected string TargetPgUrl { get; private set; }
        protected bool PageIsValid { get; set; }

        #endregion

        #region public properties

        // Implemented from IIPProxyCartridge
        public string TargetPageUrlPattern { get; private set; }
        public PageInstruction PageInstruction { get; private set; }

        public IList<IPProxy> IPProxies { get; private set; }
        public IPProxyAgentStatusEnum AgentStatus { get; set; }


        public int PageNo { get; private set; }
        public int TotalPagesScraped { get; private set; }

        #endregion

        #region constructors

        public IPProxyCartridgeBase(TContentCrawler contentCrawler, string targetPageUrlPattern, PageInstruction pgInstruction, int startPageNo)
        : base(100, true, true, false)
        {
            IPProxies = new List<IPProxy>();
            TargetPageUrlPattern = targetPageUrlPattern;
            PageInstruction = pgInstruction;
            AgentStatus = IPProxyAgentStatusEnum.Idle;
            PageNo = startPageNo == 0 ? 1 : startPageNo;

            _contentCrawler = contentCrawler;

        }
        ~IPProxyCartridgeBase()
        {
            try
            {
                //_contentGrabber.Browser.PagesAsync().Result.ForEach(pg =>
                //            {
                //                pg.CloseAsync().Wait();
                //            });
                //_contentGrabber.Browser.CloseAsync().Wait();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
            }

        }

        #endregion

        #region overrides

        /// <summary>
        /// 
        /// </summary>
        protected override void ServiceActionCallback()
        {
            if (AgentStatus == IPProxyAgentStatusEnum.Parsed)
            {
                // scrape next page after 2 minutes.
                base.Suspend(new TimeSpan(0, 0, 25));
                AgentStatus = IPProxyAgentStatusEnum.Idle;
            }

            while (!_contentCrawler.IsReady)
            {
                Thread.Sleep(0);
            }

            var usbleProx = SourceTheIPProxy();
            GrabFirstOrNextPage(usbleProx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IPProxy SourceTheIPProxy()
        {
            return null;
        }

        /// <summary>
        /// Allow the inheriting client class to validate by checking the content
        /// </summary>
        /// <param name="content"></param>
        protected virtual void ValidatePage(string content)
        {
        }

        /// <summary>
        /// Allow the inheriting client class to parse the content;
        /// From there, need to invoke this base method.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual void ParseProxyPage(string content)
        {
            AgentStatus = IPProxyAgentStatusEnum.Parsed;
            // fire the event
            InvokeEventFreeIPProxiesParsed(new EventHandlers.FreeIPProxiesParsedEventArgs(TargetPgUrl, IPProxies.ToList()));
        }

        #endregion

        #region IIPProxyProvider implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void GrabFirstOrNextPage(IPProxy proxy = null)
        {
            if (AgentStatus != IPProxyAgentStatusEnum.Idle) return;

            var pgNo = PageNo.ToString();
            if (PageInstruction != null)
                pgNo = PageInstruction.PageNo(PageNo);
            TargetPgUrl = TargetPageUrlPattern.Replace("{PAGENO}", pgNo);

            AgentStatus = IPProxyAgentStatusEnum.Reading;
            InvokeEventFreeIPProxiesReading(new EventHandlers.FreeIPProxiesReadingEventArgs(TargetPgUrl));

            // Get the html response called from the url
            var result = _contentCrawler.GrabPage(TargetPgUrl);

            if (result!=null)
            {
                var contentDoc = result.PageContent;
                
                ValidatePage(contentDoc);
                if (!PageIsValid)
                {
                    AgentStatus = IPProxyAgentStatusEnum.Completed;
                    InvokeEventFreeIPProviderSourceCompleted(
                        new EventHandlers.FreeIPProviderSourceCompletedEventArgs(PageNo));
                    base.Shutdown();
                    return;
                }

                // have the inheriting class parse the html result
                ParseProxyPage(contentDoc);
                TotalPagesScraped++;
            }

            // increase the page no to process next.
            PageNo++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        protected void RegisterProxy(IPProxy proxy)
        {
            AgentStatus = IPProxyAgentStatusEnum.Parsing;
            lock (IPProxies)
            {
                IPProxies.Add(proxy);
            }
            InvokeEventFreeIPProxyParsed(new EventHandlers.FreeIPProxyParsedEventArgs(PageNo, TargetPgUrl, proxy));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearProxyBucket()
        {
            lock (IPProxies)
            {
                IPProxies.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void RemoveProxy(IPProxy proxy)
        {
            IPProxies.ToList().Remove(proxy);
        }

        #endregion

        #region event handlers

        // implemented from IIPProxyCartridge
        public event EventHandlers.FreeIPProxyParsedEventHandler OnFreeIPProxyParsed;
        public event EventHandlers.FreeIPProxiesParsedEventHandler OnFreeIPProxiesParsed;
        public event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;
        public event EventHandlers.FreeIPProxiesReadingEventHandler OnFreeIPProxiesReading;
        public event EventHandlers.FreeIPProviderSourceCompletedEventHandler OnFreeIPProviderSourceCompleted;

        public void InvokeEventFreeIPProxyParsed(EventHandlers.FreeIPProxyParsedEventArgs a)
        {
            OnFreeIPProxyParsed?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProxiesParsed(EventHandlers.FreeIPProxiesParsedEventArgs a)
        {
            OnFreeIPProxiesParsed?.Invoke(this, a);
        }

        public void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a)
        {
            OnFreeIPGeneratorException?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProxiesReading(EventHandlers.FreeIPProxiesReadingEventArgs a)
        {
            OnFreeIPProxiesReading?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProviderSourceCompleted(EventHandlers.FreeIPProviderSourceCompletedEventArgs a)
        {
            OnFreeIPProviderSourceCompleted?.Invoke(this, a);
        }

        #endregion

    }
}
