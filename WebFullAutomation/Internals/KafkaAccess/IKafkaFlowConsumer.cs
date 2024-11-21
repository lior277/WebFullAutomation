using System.Threading.Tasks;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public interface IKafkaFlowConsumer
{
    Task StartConsuming();
    Task DeleteConsumerGroup();
    Task StopConsuming();
    T ChangeContext<T>() where T : class;
}