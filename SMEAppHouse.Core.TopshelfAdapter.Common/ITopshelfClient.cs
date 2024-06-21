using Microsoft.Extensions.Logging;

namespace SMEAppHouse.Core.TopshelfAdapter.Common
{
    public interface ITopshelfClient
    {
        InitializationStatusEnum InitializationStatus { get; set; }
        bool IsPaused { get; }
        bool IsResumed { get; }
        bool IsTerminated { get; }

        public RuntimeBehaviorOptions RuntimeBehaviorOptions { get; set; }
        ILogger Logger { get; }

        void Resume();
        void Suspend();
        void Shutdown();

        /*void Log(string data);
        void Log(string data, bool includeWriteToConsole);
        void Log(NLogLevelEnum nLogLevel, string data);
        void Log(NLogLevelEnum nLogLevel, string data, bool includeWriteToConsole);*/

    }
}