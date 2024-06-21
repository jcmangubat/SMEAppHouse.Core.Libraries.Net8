using Microsoft.Extensions.Logging;
using NodaTime;
using SMEAppHouse.Core.Scheduler;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter.Scheduler
{
    public class Scheduler : TopshelfSocket<Scheduler>
    {
        public event ScheduleReachedEventHandler OnScheduleReached = delegate { };

        public Schedule[] Schedules { get; set; }
        public Duration Duration { get; set; }
        public Schedule LastScheduleReached { get; set; }

        #region constructors

        public Scheduler(RuntimeBehaviorOptions runtimeBehaviorOptions, ILogger logger, Schedule[] schedules, Duration duration)
            : base(runtimeBehaviorOptions, logger)
        {
            Schedules = schedules;
            Duration = duration;
        }

        #endregion

        protected override void ServiceInitializeCallback()
        {
            //throw new NotImplementedException();
        }

        protected override void ServiceTerminateCallback()
        {
            //throw new NotImplementedException();
        }

        protected override void ServiceActionCallback()
        {
            var newSched = Helpers.GetScheduleOfTheMoment(this.Schedules, this.Duration);

            // if no schedule is hit or last detected schedule is same as this one, exit!
            if (newSched == null || (LastScheduleReached != null && LastScheduleReached.Id == newSched.Id))
                return;

            (new ScheduleReachedEventArg(newSched, LastScheduleReached)).InvokeEvent(this, OnScheduleReached);
            LastScheduleReached = newSched;
        }
    }
}


