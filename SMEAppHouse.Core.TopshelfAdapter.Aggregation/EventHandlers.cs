// ReSharper disable InconsistentNaming

using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter.Aggregation
{

    #region Service Initializer

    public class ServiceWorkerInitializedEventArgs(ITopshelfClient serviceWorker) : EventArgs
    {
        public ITopshelfClient ServiceWorker { get; private set; } = serviceWorker;
    }

    public delegate void ServiceWorkerInitializedEventHandler(object sender, ServiceWorkerInitializedEventArgs e);

    #endregion
    
    public static class EventHandlers
    {
        public static void InvokeEvent(this ServiceWorkerInitializedEventArgs e, object sender, ServiceWorkerInitializedEventHandler handler)
        {
            handler?.Invoke(sender, e);
        }
        
    }
}
