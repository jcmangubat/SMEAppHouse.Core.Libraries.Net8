using System.Collections.Generic;
using SMEAppHouse.Core.ScraperBox.Base;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.ScraperBox.Interfaces
{
    public interface IContentGenerator
    {
        bool IsBusy { get; set; }
        bool UseProxyWhenAvailable { get; set; }
        List<HtmlTarget> Sources { get; set; }
        void FeedSource(HtmlTarget target);

        event EventHandlers.StartingEventHandler OnStarting;
        event EventHandlers.DoneEventHandler OnDone;
        event EventHandlers.OperationExceptionEventHandler OnOperationException;

        void InvokeEventStartingEventHandler(EventHandlers.StartingEventArgs a);
        void InvokeEventDoneEventHandler(EventHandlers.DoneEventArgs a);
        void InvokeEventOperationExceptionEventHandler(EventHandlers.OperationExceptionEventArgs a);
    }
}

