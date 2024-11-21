using System.Threading.Tasks;
using LSports.FixtureManager.Abstractions.ProviderEvents.DTOs;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public interface IKafkaMessageMediator
{
    TaskCompletionSource<ProviderEventDto> Subscribe(string searchingKeyForKafka);
    bool SetFixture(ProviderEventDto message);
    void UnSubscribe(string guid);
    T ChangeContext<T>() where T : class;
}