using AutoMapper;
using SMEAppHouse.Core.AppMgt.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace SMEAppHouse.Core.AppMgt.ServiceTemplate
{
    public interface IServiceStarter<T>
        where T : class
    {
        IConfiguration Configuration { get; set; }
        ILogger<T> Logger { get; set; }
        IMapper Mapper { get; set; }
        IPayloadsEnvelope PayloadsEnvelope { get; set; }
        int TaskIntervalInSeconds { get; set; }
        void PerformServiceTask();
        Task Execute(CancellationToken cancellationToken);
        Task Terminate(CancellationToken cancellationToken);
        ServicePulseBehaviorEnum PulseBehavior { get; set; }
    }

    public enum ServicePulseBehaviorEnum
    {
        Synchronous,
        Asynchronous
    }

}