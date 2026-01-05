using System;
using SMEAppHouse.Core.ProcessService.Engines;

namespace SMEAppHouse.Core.ProcessService.POCTest
{
    internal class ProcAgentViaTaskPOCInstance : ProcessAgentViaThread
    {
        public delegate void TickEventHandler(object sender, EventArgs e);
        public event TickEventHandler OnTick;

        internal ProcAgentViaTaskPOCInstance(TimeSpan intervalTimeout)
            : base(intervalTimeout)
        {

        }

        protected override void ServiceActionCallback()
        {
            OnTick?.Invoke(this, new EventArgs());
        }

    }
}