using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter
{

    public abstract class TopshelfSocket<T> : ITopshelfClientExt where T : class
    {
        #region private variables to keep values

        private AppDotTicker<T> _ticker;

        private readonly object _mutex = new();
        private readonly AutoResetEvent _pauseEvent = new(false);
        private readonly AutoResetEvent _resumeEvent = new(false);
        private readonly AutoResetEvent _stopEvent = new(false);
        private readonly AutoResetEvent _waitEvent = new(false);

        #endregion

        #region properties

        public InitializationStatusEnum InitializationStatus { get; set; }
        public bool IsPaused { get; private set; }
        public bool IsResumed { get; private set; }
        public bool IsTerminated { get; private set; }
        public ILogger Logger { get; private set; }

        public RuntimeBehaviorOptions RuntimeBehaviorOptions { get; set; }

        public event ServiceInitializedEventHandler OnServiceInitialized;

        /// <summary>
        /// Reference to the actual thread this object is using.
        /// </summary>
        public Thread ServiceThread => new(ServiceLoop)
        {
            IsBackground = RuntimeBehaviorOptions.IsBackground
        };

        #endregion

        #region constructors

        protected TopshelfSocket(RuntimeBehaviorOptions runtimeBehaviorOptions, ILogger logger)
        {
            Logger = logger;
            RuntimeBehaviorOptions = runtimeBehaviorOptions;

            InitializeConsoleTicker();

            Task.Run(() =>
            {
                if (RuntimeBehaviorOptions.LazyInitialization)
                    return;

                TryInitialize();
                ServiceThread.Start();
            });
        }

        #endregion

        #region public methods

        public void Resume()
        {
            new Thread(() =>
            {
                if (RuntimeBehaviorOptions.LazyInitialization)
                {
                    TryInitialize();
                    ServiceThread.Start();
                }

                _resumeEvent.Set();
                _ticker.Resume();

                IsPaused = false;
                IsResumed = true;
                IsTerminated = false;

            })
            { IsBackground = true }
          .Start();
        }

        public void Suspend()
        {
            _pauseEvent.Set();
            _waitEvent.WaitOne(0);
            _ticker.Stop();

            IsPaused = true;
            IsResumed = false;
            IsTerminated = false;
        }

        public void Shutdown()
        {
            TryDestroy();
            _ticker.Shutdown();

            IsPaused = false;
            IsResumed = false;
            IsTerminated = true;

        }

        /*public void Log(string data)
        {
            Log(data, false);
        }

        public void Log(string data, bool includeWriteToConsole)
        {
            Log(NLogLevelEnum.Info, data, includeWriteToConsole);
        }

        public void NLog(NLogLevelEnum nLogLevel, string data)
        {
            Log(nLogLevel, data, false);
        }

        public void NLog(NLogLevelEnum nLogLevel, string data, bool includeWriteToConsole)
        {
            switch (nLogLevel)
            {
                case NLogLevelEnum.Fatal:
                    Logger?.Fatal(data);
                    break;
                case NLogLevelEnum.Error:
                    Logger?.Error(data);
                    break;
                case NLogLevelEnum.Warn:
                    Logger?.Warn(data);
                    break;
                case NLogLevelEnum.Info:
                    Logger?.Info(data);
                    break;
                case NLogLevelEnum.Debug:
                    Logger?.Debug(data);
                    break;
                case NLogLevelEnum.Trace:
                    Logger?.Trace(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nLogLevel), nLogLevel, null);
            }

            if (includeWriteToConsole) Console.WriteLine(data);
        }*/

        #endregion

        #region private methods

        /// <summary>
        /// 
        /// </summary>
        private void TryInitialize()
        {
            if (InitializationStatus == InitializationStatusEnum.Initialized | InitializationStatus == InitializationStatusEnum.Initializing) return;

            InitializationStatus = InitializationStatusEnum.Initializing;
            ServiceInitializeCallback();
            InitializationStatus = InitializationStatusEnum.Initialized;

            (new ServiceInitializedEventArgs()).InvokeEvent(this, OnServiceInitialized);
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryDestroy()
        {
            if (InitializationStatus != InitializationStatusEnum.Initialized)
                return;
            ServiceTerminateCallback();
            InitializationStatus = InitializationStatusEnum.NonState;
        }

        /// <summary>
        /// Our fancy ticking dots
        /// </summary>
        private void InitializeConsoleTicker()
        {
            var ctr = 0;
            _ticker = new AppDotTicker<T>(100)
            {
                OnCompletionEvent = () => Console.WriteLine(@"Press ENTER to exit!"),
                OnTickEvent = () =>
                {
                    ctr++;
                    switch (ctr % 4)
                    {
                        case 0:
                            Console.Write("\r/");
                            ctr = 0;
                            break;
                        case 1:
                            Console.Write("\r-");
                            break;
                        case 2:
                            Console.Write("\r\\");
                            break;
                        case 3:
                            Console.Write("\r|");
                            break;
                    }

                    //Console.Write(@"."
                }
            };
        }

        protected abstract void ServiceInitializeCallback();
        protected abstract void ServiceTerminateCallback();
        protected abstract void ServiceActionCallback();

        /// <summary>
        /// 
        /// </summary>
        private void ServiceLoop()
        {
            lock (_mutex)
            {
                Thread.Sleep(5000);
                do
                {
                    if (InitializationStatus != InitializationStatusEnum.Initialized)
                        continue;

                    ServiceActionCallback();

                    if (_pauseEvent.WaitOne(RuntimeBehaviorOptions.MilliSecsDelay))
                    {
                        _waitEvent.Set();
                        _resumeEvent.WaitOne(Timeout.Infinite);
                    }
                    Thread.Sleep(1);
                } while (!_stopEvent.WaitOne(0));
            }
        }

        #endregion
    }
}
