using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SMEAppHouse.Core.CodeKits;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.ProcessService.Engines;
using SMEAppHouse.Core.ScraperBox.Base;
using SMEAppHouse.Core.ScraperBox.Interfaces;
using SMEAppHouse.Core.ScraperBox.Models;
using ScrapySharp.Network;

namespace SMEAppHouse.Core.ScraperBox
{
    /// <summary>
    /// Queue-based HTML content generator that processes URLs from a queue using ProcessAgentViaTask
    /// </summary>
    public class ContentGenerator : ProcessAgentViaTask, IContentGenerator
    {
        private readonly Func<IPProxy> _proxyProvider;

        #region constructors

        public ContentGenerator()
            : base(500)
        {
            Sources = new List<HtmlTarget>();
        }

        public ContentGenerator(Func<IPProxy> proxyProvider)
            : this()
        {
            _proxyProvider = proxyProvider;
        }

        #endregion

        public List<HtmlTarget> Sources { get; set; }

        public bool UseProxyWhenAvailable { get; set; }

        /// <summary>
        /// Feed source into the queue
        /// </summary>
        /// <param name="target"></param>
        public void FeedSource(HtmlTarget target)
        {
            lock (Sources)
            {
                Sources.Add(target);
            }
        }

        public bool IsBusy { get; set; }

        #region event handlers

        public event EventHandlers.StartingEventHandler OnStarting;
        public event EventHandlers.DoneEventHandler OnDone;
        public event EventHandlers.OperationExceptionEventHandler OnOperationException;

        public void InvokeEventStartingEventHandler(EventHandlers.StartingEventArgs a)
        {
            OnStarting?.Invoke(this, a);
        }

        public void InvokeEventDoneEventHandler(EventHandlers.DoneEventArgs a)
        {
            OnDone?.Invoke(this, a);
        }

        public void InvokeEventOperationExceptionEventHandler(EventHandlers.OperationExceptionEventArgs a)
        {
            OnOperationException?.Invoke(this, a);
        }

        #endregion

        /// <summary>
        /// Iterate through all of the sources in queue
        /// </summary>
        protected override void ServiceActionCallback()
        {
            IsBusy = true;
            var target = new HtmlTarget();

            lock (Sources)
            {
                if (!Sources.Any()) 
                {
                    IsBusy = false;
                    return;
                }
                var ttarget = Sources.First();

                CodeKit.CopyObjectProperties(ttarget, target);

                Sources.Remove(ttarget);
            }

            if (!ProcessSource(target))
            {
                lock (Sources)
                {
                    Sources.Add(target);
                }
            }
            IsBusy = false;
        }

        /// <summary>
        /// Process a single source target
        /// </summary>
        /// <param name="target"></param>
        private bool ProcessSource(HtmlTarget target)
        {
            // get event argument on startup
            var arg = new EventHandlers.StartingEventArgs(target);
            InvokeEventStartingEventHandler(arg);
            if (arg.Cancel) return false;

            // start computing for the elapse time
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var proxyIsInvalid = false;

            IPProxy freeProxy = null;
            FakeUserAgent fakeUserAgent = null;

            if (_proxyProvider != null && UseProxyWhenAvailable)
            {
                try
                {
                    freeProxy = _proxyProvider();
                    fakeUserAgent = UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280);
                }
                catch
                {
                    // Proxy provider failed, continue without proxy
                }
            }

            try
            {
                target.Url = Helper.ResolveHttpUrl(target.Url);
                target.Content = GetContent(target.Url, fakeUserAgent, freeProxy, ref proxyIsInvalid);

                // If the proxy is returned invalid, the scraping have been unsuccessful with its use. 
                // Give it back to the sources collection to reprocess.
                if (proxyIsInvalid)
                {
                    lock (Sources) { Sources.Add(target); }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("501") || ex.Message.Contains("503"))
                    target.PageIsInvalid = true;

                InvokeEventOperationExceptionEventHandler(new EventHandlers.OperationExceptionEventArgs(ex));
            }

            var done = new EventHandlers.DoneEventArgs(target, freeProxy, stopwatch.Elapsed);
            InvokeEventDoneEventHandler(done);

            stopwatch.Stop();
            return true;
        }

        /// <summary>
        /// Get content from URL with optional proxy and user agent
        /// </summary>
        /// <param name="pgUrl"></param>
        /// <returns></returns>
        public string GetContent(string pgUrl)
        {
            IPProxy freeProxy = null;
            FakeUserAgent fakeUserAgent = null;

            if (_proxyProvider != null && UseProxyWhenAvailable)
            {
                try
                {
                    freeProxy = _proxyProvider();
                    fakeUserAgent = UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280);
                }
                catch
                {
                    // Proxy provider failed, continue without proxy
                }
            }

            var prxyIsInvalid = false;
            return GetContent(pgUrl, fakeUserAgent, freeProxy, ref prxyIsInvalid);
        }

        /// <summary>
        /// Get content from URL with proxy and user agent support, with retry logic
        /// </summary>
        /// <param name="pgUrl"></param>
        /// <param name="fakeUserAgent"></param>
        /// <param name="freeProxy"></param>
        /// <param name="proxyInvalid"></param>
        /// <returns></returns>
        private static string GetContent(string pgUrl, FakeUserAgent fakeUserAgent, IPProxy freeProxy, ref bool proxyInvalid)
        {
            try
            {
                IWebProxy proxy = null;
                if (freeProxy != null)
                    proxy = freeProxy.ToWebProxy();

                var htmlDoc = Helper.GetPageDocument(new Uri(pgUrl), proxy, fakeUserAgent);
                return htmlDoc;
            }
            catch (Exception ex)
            {
                // Check if proxy is invalid
                if (ex.Message.Contains("proxy is invalid") || ex.Message.Contains("failed to respond"))
                {
                    proxyInvalid = true;
                }
                throw;
            }
        }
    }
}

