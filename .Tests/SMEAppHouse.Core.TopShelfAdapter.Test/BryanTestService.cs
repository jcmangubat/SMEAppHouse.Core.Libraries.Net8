using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.TopshelfAdapter;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopShelfAdapter.Test;

public class BryanTestService : TopshelfSocket<BryanTestService>
{

    private int _testVal;

    public BryanTestService(RuntimeBehaviorOptions runtimeBehaviorOptions, ILogger logger) :
        base(runtimeBehaviorOptions, logger)
    {
    }

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
        _testVal++;

        Logger.LogInformation($"current value: {_testVal}\r\n");
    }
}